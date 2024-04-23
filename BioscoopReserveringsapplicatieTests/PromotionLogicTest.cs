using NSubstitute;

namespace BioscoopReserveringsapplicatie
{
    [TestClass]
    public class PromotionLogicTest
    {
        public PromotionLogic Initialize()
        {
            var promotionRepositoryMock = Substitute.For<IDataAccess<PromotionModel>>();
            List<PromotionModel> promotions = new List<PromotionModel>() {
                new PromotionModel(1, "Promotion 1", "Description 1", false),
                new PromotionModel(2, "Promotion 2", "Description 2", true),
                new PromotionModel(3, "Promotion 3", "Description 3", false),
            };
            promotionRepositoryMock.LoadAll().Returns(promotions);
            promotionRepositoryMock.WriteAll(Arg.Any<List<PromotionModel>>());

            return new PromotionLogic(promotionRepositoryMock);
        }

        // Title ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Promotion_Title_Validation(string title)
        {
            PromotionLogic promotionLogic = Initialize();
            Assert.IsTrue(promotionLogic.ValidateTitle(title));
        }

        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void Incorrect_Promotion_Title_Validation_With_Promotion(string title)
        {
            PromotionLogic promotionLogic = Initialize();
            PromotionModel promotion = new PromotionModel(1, title, "Description", false);
            Assert.IsFalse(promotionLogic.Validate(promotion));
            PromotionModel promotion2 = new PromotionModel(2, title, "Description", false);
            Assert.IsFalse(promotionLogic.Validate(promotion2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Promotion_Title_Validation(string title)
        {
            PromotionLogic promotionLogic = Initialize();
            Assert.IsFalse(promotionLogic.ValidateTitle(title));
        }

        // Description ------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyDescription")]
        [DataTestMethod]
        public void Correct_Promotion_Description_Validation(string description)
        {
            PromotionLogic promotionLogic = Initialize();
            Assert.IsTrue(promotionLogic.ValidateDescription(description));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Promotion_Description_Validation_With_Promotion(string description)
        {
            PromotionLogic promotionLogic = Initialize();
            PromotionModel promotion = new PromotionModel(1, "Title", description, false);
            Assert.IsFalse(promotionLogic.Validate(promotion));
            PromotionModel promotion2 = new PromotionModel(2, "Title", description, false);
            Assert.IsFalse(promotionLogic.Validate(promotion2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Promotion_Description_Validation(string description)
        {
            PromotionLogic promotionLogic = Initialize();
            Assert.IsFalse(promotionLogic.ValidateDescription(description));
        }

        // Status ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Activate()
        {
            PromotionLogic promotionLogic = Initialize();
            promotionLogic.Activate(1);
            Assert.IsTrue(promotionLogic.GetById(1).Status);
        }

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Deactivate()
        {
            PromotionLogic promotionLogic = Initialize();
            promotionLogic.Deactivate(2);
            Assert.IsFalse(promotionLogic.GetById(2).Status);
        }
    }
}