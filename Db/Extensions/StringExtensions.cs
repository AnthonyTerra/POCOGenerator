using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Db.Extensions
{
    public static class StringExtensions
    {
        private static readonly PluralizationService service = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-US"));

        static StringExtensions()
        {
            var mapping = (ICustomPluralizationMapping)service;
            mapping.AddWord("Cactus", "Cacti");
            mapping.AddWord("cactus", "cacti");
            mapping.AddWord("Die", "Dice");
            mapping.AddWord("die", "dice");
            mapping.AddWord("Equipment", "Equipment");
            mapping.AddWord("equipment", "equipment");
            mapping.AddWord("Money", "Money");
            mapping.AddWord("money", "money");
            mapping.AddWord("Nucleus", "Nuclei");
            mapping.AddWord("nucleus", "nuclei");
            mapping.AddWord("Quiz", "Quizzes");
            mapping.AddWord("quiz", "quizzes");
            mapping.AddWord("Shoe", "Shoes");
            mapping.AddWord("shoe", "shoes");
            mapping.AddWord("Syllabus", "Syllabi");
            mapping.AddWord("syllabus", "syllabi");
            mapping.AddWord("Testis", "Testes");
            mapping.AddWord("testis", "testes");
            mapping.AddWord("Virus", "Viruses");
            mapping.AddWord("virus", "viruses");
            mapping.AddWord("Water", "Water");
            mapping.AddWord("water", "water");
        }

        public static string ToSingular(this string word)
        {
            if (word == null)
                throw new ArgumentNullException("word");

            bool isUpperWord = (string.Compare(word, word.ToUpper(), false) == 0);
            if (isUpperWord)
            {
                string lowerWord = word.ToLower();
                return (service.IsSingular(lowerWord) ? lowerWord : service.Singularize(lowerWord)).ToUpper();
            }

            return (service.IsSingular(word) ? word : service.Singularize(word));
        }

        public static string ToPlural(this string word)
        {
            if (word == null)
                throw new ArgumentNullException("word");

            bool isUpperWord = (string.Compare(word, word.ToUpper(), false) == 0);
            if (isUpperWord)
            {
                string lowerWord = word.ToLower();
                return (service.IsPlural(lowerWord) ? lowerWord : service.Pluralize(lowerWord)).ToUpper();
            }

            return (service.IsPlural(word) ? word : service.Pluralize(word));
        }
    }
}
