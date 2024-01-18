public class KeywordActivatableModifier : BaseActivatableModifier
{
    public KeywordModifier[] keywords;
    public override bool activatable
    {
        get
        {
            if (keywords.Length == 0) return true;
            for (int i = 0; i < keywords.Length; i++)
            {
                var item = keywords[i];
                if (item.modifier == KeywordModifier.Modifier.Restrict && ScenarioManager.ActiveKeywords.Contains(item.keyword)) return false;
                if (item.modifier == KeywordModifier.Modifier.Require && !ScenarioManager.ActiveKeywords.Contains(item.keyword)) return false;
            }
            return true;
        }
    }
    public struct KeywordModifier
    {
        public string keyword;
        public Modifier modifier;
        public enum Modifier
        {
            Require,
            Restrict
        }
    }
}