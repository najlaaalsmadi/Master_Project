using backend_Master.Models;

namespace backend_Master.DTO
{
    public class CardItemDTO
    {
        public int? CardId { get; set; }      // معرف السلة (المستخدم)
        public int? ProductId { get; set; }   // معرف المنتج
        public int? EquipmentId { get; set; }

        public int? Quantity { get; set; }    // الكمية
        public decimal? Price { get; set; }   // السعر
        public DateTime? AddedAt { get; set; }  // تاريخ الإضافة


    }
    public interface ICardService
    {
        CardItem AddOrUpdateCardItem(CardItemDTO cardItemDto);
        bool IsProductInCard(int cardId, int productId);
        //bool IsEquipmentInCard(int cardId, int equipmentId);  // إضافة هذه الدالة للتحقق من المعدات

    }

}
