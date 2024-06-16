namespace ChefHesab.Share.model
{
    public class ChefResult
    {

        public object data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; } = new List<string>();

        public void AddError(string message)
        {
            IsSuccess = false;
            Errors.Add(message);
         }
    }
}