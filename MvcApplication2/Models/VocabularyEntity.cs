using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgrajKarte.Models
{
    public class VocabularyEntity
    {
        private string engWord;
        private string translate;
        private string synonym;

        public string Synonym
        {
            get { return synonym; }
            set { synonym = value; }
        }

        public string Translate
        {
            get { return translate; }
            set { translate = value; }
        }

        public string EngWord
        {
            get { return engWord; }
            set { engWord = value; }
        }

        public VocabularyEntity()
        { 
            
        }

        public VocabularyEntity(string engWord, string translate, string synonym)
        {
            EngWord = engWord;
            Translate = translate;
            Synonym = synonym;
        }
    }
}