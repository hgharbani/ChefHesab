namespace ChefHesab.Share
{
    public class FieldInfos
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldCondition { get; set; }
        
        public dynamic FieldConditionValue { get; set; }
    }

    public class FormulsParametes
    {
        public string ParametersName { get; set; }
        public string ParametersValue { get; set; }
    }
}
