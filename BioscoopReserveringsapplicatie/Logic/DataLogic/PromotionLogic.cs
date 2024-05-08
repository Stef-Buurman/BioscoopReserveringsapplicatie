namespace BioscoopReserveringsapplicatie
{
    public class PromotionLogic : ILogic<PromotionModel>
    {
        private List<PromotionModel> _promotions = new();
        public IDataAccess<PromotionModel> _DataAccess { get; }
        public PromotionLogic(IDataAccess<PromotionModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<PromotionModel>();

            _promotions = _DataAccess.LoadAll();
        }

        public List<PromotionModel> GetAll()
        {
            _promotions = _DataAccess.LoadAll();
            return _promotions;
        }

        public PromotionModel? GetById(int id)
        {
            _promotions = _DataAccess.LoadAll();
            return _promotions.Find(s => s.Id == id);
        }

        public PromotionModel? GetByStatus(bool status)
        {
            _promotions = _DataAccess.LoadAll();
            return _promotions.Find(s => s.Status == status);
        }

        public int GetNextId() => IdGenerator.GetNextId(_promotions);

        public bool Add(PromotionModel promotion)
        {
            GetAll();

            if (!Validate(promotion))
            {
                return false;
            }
            UpdateList(promotion);
            return true;
        }

        public bool Edit(PromotionModel promotion)
        {
            // This will be done in the near future
            return true;
        }

        public void Activate(int id)
        {
            PromotionModel? promotion = GetById(id);

            if (promotion != null)
            {
                promotion.Status = true;
                UpdateList(promotion);
            }
        }

        public void Deactivate(int id)
        {
            PromotionModel? promotion = GetById(id);

            if (promotion != null)
            {
                promotion.Status = false;
                UpdateList(promotion);
            }
        }

        public bool Validate(PromotionModel promotion)
        {
            if (promotion == null) return false;
            else if (!ValidateTitle(promotion.Title)) return false;
            else if (!ValidateDescription(promotion.Description)) return false;
            return true;
        }

        public bool ValidateTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        public bool ValidateDescription(string description)
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
            _DataAccess.WriteAll(_promotions);
        }
        public PromotionModel? GetActivePromotion()
        {
            _promotions = _DataAccess.LoadAll();
            return _promotions.Find(s => s.Status == true);
        }
    }
}