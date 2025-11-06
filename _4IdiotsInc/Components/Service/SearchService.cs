namespace _4IdiotsInc.Components.Service
{
    public class SearchService
    {
        private string _searchQuery = string.Empty;

        public event Action<string>? OnSearchChanged;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnSearchChanged?.Invoke(_searchQuery);
                }
            }
        }
    }
}
