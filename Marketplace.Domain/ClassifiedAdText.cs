namespace Marketplace.Domain
{
    public class ClassifiedAdText
    {
        protected ClassifiedAdText() { }
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
        internal ClassifiedAdText(string text) => Value = text;

        public string Value { get; internal set; }

        
        public static implicit operator string(ClassifiedAdText text) =>
            text.Value;
     
        public static ClassifiedAdText NoText = new ClassifiedAdText();

    }
}