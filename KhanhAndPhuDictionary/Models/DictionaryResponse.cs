using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace KhanhAndPhuDictionary.Models
{
    public class DictionaryResponse
    {
        public string Word { get; set; } = string.Empty;
        public string Phonetic { get; set; } = string.Empty;
        public List<Phonetic> Phonetics { get; set; } = new();
        public List<Meaning> Meanings { get; set; } = new();
    }

    public class Phonetic
    {
        public string Text { get; set; } = string.Empty;
        public string Audio { get; set; } = string.Empty;
    }

    public class Meaning
    {
        public string PartOfSpeech { get; set; } = string.Empty;
        public List<Definition> Definitions { get; set; } = new();
        public List<string> Synonyms { get; set; } = new();
    }

    public class Definition
    {
        [JsonPropertyName("definition")]
        public string DefinitionText { get; set; } = string.Empty; 
        public string Example { get; set; } = string.Empty;
    }
}