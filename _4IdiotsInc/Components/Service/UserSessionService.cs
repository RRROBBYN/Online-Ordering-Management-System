using _4IdiotsInc.Model;

namespace _4IdiotsInc.Components.Service
{
    public class UserSessionService
    {
        private UserAccount? _currentUser;
        public event Action? OnUserChanged;

        public UserAccount? CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnUserChanged?.Invoke();
            }
        }

        public string? CurrentUserId => CurrentUser?.Id;
        public bool IsLoggedIn => CurrentUser != null;

        public void SetCurrentUser(UserAccount user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public string GetUserDisplayName()
        {
            if (CurrentUser == null) return "Guest";
            return $"{CurrentUser.Username}".Trim();
        }
    }
}