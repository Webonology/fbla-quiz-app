using System.Linq;
using System;
using System.Web.Mvc;
using fbla_app_ui.Models;
using fbla_app_ui.ViewModels;
using System.Collections.Generic;

namespace fbla_app_ui.Controllers
{
    public class HomeController : Controller
    {
        private FBLAAppEntities db = new FBLAAppEntities();

        public ActionResult Index()
        {
            int id = db.Quizs.Max(q => q.ID);
            UserQuizViewModel userQuizViewModel = new UserQuizViewModel();
            userQuizViewModel.TrackingID = Guid.NewGuid();

            Quiz quiz = db.Quizs.Where(q => q.ID == id).FirstOrDefault();

            if (quiz == null)
            {
                return HttpNotFound();
            }

            int questionCount = db.QuizQuestions.Where(q => q.QuizID == id).Count() + 1;

            userQuizViewModel.Quiz = new QuizViewModel
            {
                ID = quiz.ID,
                Name = quiz.Name,
                AmountOfQuestions = quiz.Amount
            };

            IList<int> questionNumbers = new List<int>();
            Random rand = new Random();
            int next;

            for (int i = 0; i < quiz.Amount; i++)
            {
                if (questionNumbers.Count == 0)
                {
                    next = rand.Next(1, questionCount);
                    bool exists = false;
                    while (!exists)
                    {
                        exists = db.QuizQuestions.Any(q => q.QuizID == id && q.ID == next);
                        if (!exists)
                        {
                            next = rand.Next(1, questionCount);
                        }
                    }
                }
                else
                {
                    do
                    {
                        next = rand.Next(1, questionCount);
                        bool exists = false;
                        while (!exists)
                        {
                            exists = db.QuizQuestions.Any(q => q.QuizID == id && q.ID == next);
                            if (!exists)
                            {
                                next = rand.Next(1, questionCount);
                            }
                        }
                    } while (questionNumbers.Contains(next));
                }
                questionNumbers.Add(next);
            }

            foreach (int number in questionNumbers)
            {
                UserQuestionAnswerViewModel userQuestions = db.QuizQuestions.Where(q => q.ID == number)
                    .Select(q => new UserQuestionAnswerViewModel
                    {
                        QuestionID = q.ID,
                        Question = q.Question
                    }).FirstOrDefault<UserQuestionAnswerViewModel>();

                userQuestions.Answers = db.QuizAnswers.Where(a => a.QuestionID == number)
                    .Select(a => new AnswerViewModel
                    {
                        ID = a.ID,
                        Answer = a.Answer
                    }).ToList<AnswerViewModel>();

                userQuestions.AnswerType = db.QuizAnswers.Where(a => a.QuestionID == number)
                    .Select(a => a.AnswerType).FirstOrDefault<string>();

                if (userQuizViewModel.QuizQuestionAndAnswer == null)
                {
                    userQuizViewModel.QuizQuestionAndAnswer = new List<UserQuestionAnswerViewModel>();
                }

                userQuizViewModel.QuizQuestionAndAnswer.Add(userQuestions);
            }

            return View(userQuizViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserQuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = model.Name,
                    Email = model.Email
                };
                db.Users.Add(user);
                db.SaveChanges();

                UserQuiz userQuiz = new UserQuiz
                {
                    QuizID = model.Quiz.ID,
                    UserID = user.ID
                };
                db.UserQuizs.Add(userQuiz);
                db.SaveChanges();

                foreach (var question in model.QuizQuestionAndAnswer)
                {
                    UserQuizAnswer userQuizAnswer = null;

                    if (question.AnswerType == "TX")
                    {
                        userQuizAnswer = new UserQuizAnswer
                        {
                            QuizAnswerID = question.Answers[0].ID,
                            UserQuizText = question.UserAnswer,
                            UserQuizID = userQuiz.ID
                        };
                    }
                    else
                    {
                        userQuizAnswer = new UserQuizAnswer
                        {
                            QuizAnswerID = Convert.ToInt32(question.UserAnswer),
                            UserQuizID = userQuiz.ID
                        };
                    }
                    db.UserQuizAnswers.Add(userQuizAnswer);
                    db.SaveChanges();
                }

                return RedirectToAction("Results", new { id = user.ID });
            }

            return View(model);
        }

        public ActionResult Results(int id)
        {
            UserSummaryViewModel model = db.Users.Where(u => u.ID == id)
                .Select(u => new UserSummaryViewModel
                {
                    Name = u.Name,
                    Email = u.Email
                }).SingleOrDefault<UserSummaryViewModel>();

            UserQuiz userQuiz = db.UserQuizs.Where(uq => uq.UserID == id).SingleOrDefault<UserQuiz>();

            List<UserQuizAnswer> userQuizAnswers = db.UserQuizAnswers.Where(ua => ua.UserQuizID == userQuiz.ID)
                .ToList<UserQuizAnswer>();

            model.QuestionsAndAnswers = new List<QuestionsAndAnswersViewModel>();

            foreach (var item in userQuizAnswers)
            {
                QuestionsAndAnswersViewModel data = new QuestionsAndAnswersViewModel();

                QuizAnswer answer = db.QuizAnswers.Where(a => a.ID == item.QuizAnswerID).SingleOrDefault<QuizAnswer>();

                QuizQuestion quizQuestion = db.QuizQuestions.Where(q => q.ID == answer.QuestionID).SingleOrDefault<QuizQuestion>();

                QuizAnswer correctAnswer = db.QuizAnswers.Where(a => a.QuestionID == quizQuestion.ID && a.CorrectAnswer == true).SingleOrDefault<QuizAnswer>();

                if (correctAnswer.AnswerType == "TX")
                {
                    if (correctAnswer.Answer.ToLower() == item.UserQuizText.ToLower())
                    {
                        data.Correct = true;
                    }
                    else
                    {
                        data.Correct = false;
                    }
                }
                else
                {
                    if (answer.ID == correctAnswer.ID)
                    {
                        data.Correct = true;
                    }
                    else
                    {
                        data.Correct = false;
                    }
                }
                data.CorrectAnswer = correctAnswer.Answer;
                data.Question = quizQuestion.Question;
                if (answer.AnswerType == "TX")
                {
                    data.UserAnswer = item.UserQuizText;
                }
                else
                {
                    data.UserAnswer = answer.Answer;
                }
                data.WhyCorrect = correctAnswer.WhyCorrectAnswer;

                model.QuestionsAndAnswers.Add(data);
            }

            model.NumberOfCorrect = model.QuestionsAndAnswers.Where(m => m.Correct == true).Count();
            model.NumberOfQuestions = model.QuestionsAndAnswers.Count();

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}