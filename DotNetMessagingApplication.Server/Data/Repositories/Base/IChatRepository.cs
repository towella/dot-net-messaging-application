using DotNetMessagingApplication.Server.Data.Models;

namespace DotNetMessagingApplication.Server.Data.Repositories.Base
{
    public interface IChatRepository // this is here bc a Repository cant be instantiated for the abstract Chat class. so yeah :/
    {
        Task<Chat?> GetChatById(int ChatId);
        Task<int> DeleteChat(int chatId);
        Task<IEnumerable<Chat>> GetAllChatsForSpecificUser(int userId);
        Task<Chat> AddChat(int userId);
    }
}
