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
                new PromotionModel(1, "Promotion 1", "Description 1", Status.Inactive),
                new PromotionModel(2, "Promotion 2", "Description 2", Status.Active),
                new PromotionModel(3, "Promotion 3", "Description 3", Status.Inactive),
                new PromotionModel(4, "Promotion 4", "Description 4", Status.Archived),
                new PromotionModel(5, "Promotion 5", "Description 5", Status.Active),
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
            PromotionModel promotion = new PromotionModel(1, title, "Description", Status.Inactive);
            Assert.IsFalse(promotionLogic.Validate(promotion));
            PromotionModel promotion2 = new PromotionModel(2, title, "Description", Status.Inactive);
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
            PromotionModel promotion = new PromotionModel(1, "Title", description, Status.Inactive);
            Assert.IsFalse(promotionLogic.Validate(promotion));
            PromotionModel promotion2 = new PromotionModel(2, "Title", description, Status.Inactive);
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
            Assert.AreEqual(promotionLogic.GetById(1).Status, Status.Active);
        }

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Deactivate()
        {
            promotionLogic.Deactivate(2);
            Assert.AreEqual(promotionLogic.GetById(2).Status, Status.Inactive);
        }

        // Archive ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Archive_When_Active()
        {
            promotionLogic.Archive(3);
            Assert.AreEqual(promotionLogic.GetById(3).Status, Status.Archived);
        }

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Archive_When_Inactive()
        {
            promotionLogic.Archive(5);
            Assert.AreEqual(promotionLogic.GetById(5).Status, Status.Archived);
        }

        [TestMethod]
        public void Correct_Promotion_Status_Validation_Unarchive()
        {
            promotionLogic.Unarchive(4);
            Assert.AreEqual(promotionLogic.GetById(4).Status, Status.Inactive);
        }

        // Edit ------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Promotion_Edit()
        {
            Assert.IsFalse(promotionLogic.Edit(new PromotionModel(1, "Title", "Description", Status.Inactive)));
            Assert.AreEqual("Title", promotionLogic.GetById(1).Title);
            Assert.AreEqual("Description", promotionLogic.GetById(1).Description);

        }

        [TestMethod]
        public void Incorrect_Promotion_Edit()
        {
            Assert.IsFalse(promotionLogic.Edit(new PromotionModel(1, "", "", Status.Inactive)));
            Assert.AreNotEqual("", promotionLogic.GetById(1).Title);
            Assert.AreNotEqual("", promotionLogic.GetById(1).Description);
        }

        [TestMethod]
        public void Incorrect_Promotion_Edit_With_Invalid_Title()
        {
            Assert.IsFalse(promotionLogic.Edit(new PromotionModel(1, "", "Description", Status.Inactive)));
            Assert.AreNotEqual("", promotionLogic.GetById(1).Title);
            Assert.AreNotEqual("Description", promotionLogic.GetById(1).Description);
        }

        [TestMethod]
        public void Incorrect_Promotion_Edit_With_Invalid_Description()
        {
            Assert.IsFalse(promotionLogic.Edit(new PromotionModel(1, "Title", "", Status.Inactive)));
            Assert.AreNotEqual("Title", promotionLogic.GetById(1).Title);
            Assert.AreNotEqual("", promotionLogic.GetById(1).Description);
        }
    }
}