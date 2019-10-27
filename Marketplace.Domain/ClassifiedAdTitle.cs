using System;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        protected ClassifiedAdTitle()
        {
        }

        public static ClassifiedAdTitle FromString(string title)
        {
            CheckValidity(title);
            return new ClassifiedAdTitle(title);
        }

        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException("Title cannot be longer than 100 characters", nameof(value));
            }
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "*")
                .Replace("</b>", "*");
            var value = new ClassifiedAdTitle(Regex.Replace(supportedTagsReplaced, "<.*?>", String.Empty));
            CheckValidity(value);
            return value;
        }

        internal ClassifiedAdTitle(string value) => Value = value;

        public static implicit operator string(ClassifiedAdTitle title) =>
            title.Value;

        public string Value { get; }
    }
}