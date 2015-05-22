using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgrajKarte.Models;
using System.IO;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Threading.Tasks;

namespace IgrajKarte.Controllers
{
    [AllowAnonymous]
    public class VocabularyController : Controller
    {
        private List<VocabularyEntity> wordsList;
        private bool lockSpeach;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Words()
        {
            lockSpeach = false;
            string path = @"C:\Users\Ljubisa\Documents\Notepad++\Vocabulary.txt";
            wordsList = new List<VocabularyEntity>();
            string[] separators = {" - ", " // "};
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    VocabularyEntity word;
                    string[] attributes = sr.ReadLine().Split(separators, StringSplitOptions.None);
                    if (attributes.Count() > 2)
                    {
                        if (attributes.Count() == 3)
                            word = new VocabularyEntity(attributes[0], attributes[1], attributes[2]);
                        else
                            word = new VocabularyEntity(attributes[0], attributes[1], "");
                     
                        wordsList.Add(word);
                    }
                }
            }

            #region Load .Net defatult grammer.
            /*
            // Load Dictation Grammer 
            DictationGrammar dictationGrammer = new DictationGrammar();
            _speechRecognitionEngine.LoadGrammar(dictationGrammer);
            */
            PromptBuilder p = new PromptBuilder();
            p.AppendText("assignment");
            p.AppendTextWithPronunciation("assignment", "duˈbwɑ");

            #endregion

            return View(wordsList);
        }

        public async Task<ActionResult> Speak(string word)
        {
            if (!lockSpeach)
            {
                lockSpeach = true;
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                synthesizer.Volume = 100;  // 0...100
                synthesizer.Rate = -2;     // -10...10

                synthesizer.Speak(word);
                lockSpeach = false;
            }
            return Json(new { status = true });
        }
    }
}