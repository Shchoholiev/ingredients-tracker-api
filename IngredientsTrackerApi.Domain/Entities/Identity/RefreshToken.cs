using IngredientsTrackerApi.Domain.Common;

namespace IngredientsTrackerApi.Domain.Entities.Identity;

public class RefreshToken : EntityBase
{
    public string Token { get; set; }

    public DateTime ExpiryDateUTC { get; set; }
}
