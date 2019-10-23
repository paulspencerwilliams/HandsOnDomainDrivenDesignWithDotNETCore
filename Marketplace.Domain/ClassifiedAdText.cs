namespace Marketplace.Domain
{
    public class ClassifiedAdText
    {
        internal ClassifiedAdText(string text) => Value = text;

        public string Value { get;  }

        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
    }
}