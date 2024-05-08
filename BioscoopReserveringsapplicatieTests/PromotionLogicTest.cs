using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class PromotionLogicTest
    {
        PromotionLogic promotionLogic;

        [TestInitialize]
        public void Initialize()
        {
            var promotionRepositoryMock = Substitute.For<IDataAccess<PromotionModel>>();
            List<PromotionModel> promotions = new List<PromotionModel>() {
                new PromotionModel(1, "Promotion 1", "Description 1", false),
                new PromotionModel(2, "Promotion 2", "Description 2", true),
                new PromotionModel(3, "Promotion 3", "Description 3", false),
            };
            promotionRepositoryMock.LoadAll().Returns(promotions);
            promotionRepositoryMock.WriteAll(Arg.Any<List<PromotionModel>>());

            promotionLogic = new PromotionLogic(promotionRepositoryMock);
        }

        // Title ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Promotion_Title_Validation(string title)
        {
            Assert.IsTrue(promotionLogic.ValidateTitle(title));
        }

        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void Incorrect_Promotion_Title_Validation_With_Promotion(string title)
        {
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
            Assert.IsFalse(promotionLogic.ValidateTitle(title));
        }

        // Description ------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyDescription")]
        [DataTestMethod]
        public void Correct_Promotion_Description_Validation(string description)
        {
            Assert.IsTrue(promotionLogic.ValidateDescription(description));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Promotion_Description_Validation_With_Promotion(string description)
        {
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
            Assert.IsFalse(promotionLogic.ValidateDescription(description));
        }

        // Status ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Activate()
        {
            promotionLogic.Activate(1);
            Assert.IsTrue(promotionLogic.GetById(1).Status);
        }

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Deactivate()
        {
            promotionLogic.Deactivate(2);
            Assert.IsFalse(promotionLogic.GetById(2).Status);
        }

        // Edit ------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Promotion_Edit()
        {
            promotionLogic.Edit(new PromotionModel(1, "Title", "Description", false));
            Assert.AreEqual("Title", promotionLogic.GetById(1).Title);
            Assert.AreEqual("Description", promotionLogic.GetById(1).Description);
        }

        [TestMethod]
        public void Incorrect_Promotion_Edit()
        {
            promotionLogic.Edit(new PromotionModel(1, "", "", false));
            Assert.AreNotEqual("NeTitle", promotionLogic.GetById(1).Title);
            Assert.AreNotEqual("NewDescription", promotionLogic.GetById(1).Description);
        }

        [TestMethod]
        public void Incorrect_Promotion_Edit_With_Invalid_Title()
        {
            promotionLogic.Edit(new PromotionModel(1, "", "Description", false));
            Assert.AreNotEqual("Title", promotionLogic.GetById(1).Title);
            Assert.AreNotEqual("Description", promotionLogic.GetById(1).Description);
        }

        [TestMethod]
        public void Incorrect_Promotion_Edit_With_Invalid_Description()
        {
            promotionLogic.Edit(new PromotionModel(1, "Title", "", false));
            Assert.AreNotEqual("Title", promotionLogic.GetById(1).Title);
            Assert.AreNotEqual("Description", promotionLogic.GetById(1).Description);
        }
    }
}