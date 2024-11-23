using Database.Databases;

namespace AuthorTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task When_CreateAuthorCommand_IsHandled_Then_AuthorIsAddedToList()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new CreateAuthorCommandHandler(fakeDatabase);
            var initialAuthorCount = fakeDatabase.Authors.Count;
            var newAuthor = new Author("Test", "johnnysAuthor");

            // Act
            await handler.Handle(new CreateAuthorCommand(newAuthor), CancellationToken.None);

            // Assert
            Assert.AreEqual(initialAuthorCount + 1, fakeDatabase.Authors.Count, "The authors list should now contain 1 more author than before.");
            Assert.IsTrue(fakeDatabase.Authors.Contains(newAuthor), "The new author should be in the authors list.");
        }
        {
            
        }
    }
}