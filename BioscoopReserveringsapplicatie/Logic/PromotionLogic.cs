namespace BioscoopReserveringsapplicatie
{
    public class PromotionLogic
    {
        private List<PromotionModel> _promotions = new();

        public PromotionLogic()
        {
            _promotions = PromotionAccess.LoadAll();
        }

        public List<PromotionModel> GetAll()
        {
            return _promotions;
        }

        public PromotionModel? GetById(int id)
        {
            return _promotions.Find(s => s.Id == id);
        }

        public PromotionModel? GetByStatus(bool status)
        {
            return _promotions.Find(s => s.Status == status);
        }

        public bool Add(string title, string description)
        {
            if (ValidatePromotionTitle(title) && ValidatePromotionDescription(description))
            {
                PromotionModel promotion = new PromotionModel(IdGenerator.GetNextId(_promotions), title, description, false);

                if (this.ValidatePromotion(promotion))
                {
                    UpdateList(promotion);
                    return true;
                }
            }

            return false;
        }

        public bool ValidatePromotion(PromotionModel promotion)
        {
            if (promotion == null) return false;
            else if (!ValidatePromotionTitle(promotion.Title)) return false;
            else if (!ValidatePromotionDescription(promotion.Description)) return false;
            return true;
        }

        public bool ValidatePromotionTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        public bool ValidatePromotionDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description);
        }

        public void UpdateList(PromotionModel promotion)
        {
            //Find if there is already an model with the same id
            int index = _promotions.FindIndex(s => s.Id == promotion.Id);

            if (index != -1)
            {
                //update existing model
                _promotions[index] = promotion;
            }
            else
            {
                //add new model
                _promotions.Add(promotion);
            }
            PromotionAccess.WriteAll(_promotions);
            _promotions = PromotionAccess.LoadAll();
        }
    }
}