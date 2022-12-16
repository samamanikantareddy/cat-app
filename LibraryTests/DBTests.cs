using Library.DataAccess;
using Library.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTests
{
    public class MockStoreWrapper
    {
        public static ApplicationUser user2 = new ApplicationUser
        {
            UserName = "User2"
        };

        public static Cat catToAdd = new Cat
        {
            id = "iWyIaja-G",
            Fan = user2,
            width = 1080,
            height = 769,
            url = "https://cdn2.thecatapi.com/images/iWyIaja-G.jpg",
            categories = new List<Category>
                    {
                        new Category
                        {
                            id = 2,
                            name = "clothes"
                        }
                    },
            breeds = new List<Breed>
                    {
                        new Breed
                        {
                            id = "beng",
                            name = "Bengal",
                            cfa_url = "http://cfa.org/Breeds/BreedsAB/Bengal.aspx",
                            vetstreet_url = "http://www.vetstreet.com/cats/bengal",
                            temperament = "Alert, Agile, Energetic, Demanding, Intelligent",
                            origin = "United States",
                            description = "Bengals are a lot of fun to live with, but they're definitely not the cat for everyone, or for first-time cat owners. Extremely intelligent, curious and active, they demand a lot of interaction and woe betide the owner who doesn't provide it.",
                            wikipedia_url = "https://en.wikipedia.org/wiki/Bengal_(cat)"
                        }
                    }
        };

        public static string username = "User1";

        public static Mock<IProvider<Cat>> GetMock()
        {
            var mock = new Mock<IProvider<Cat>>();

            var user1 = new ApplicationUser
            {
                UserName = username
            };

            // Setup the mock
            var cats = new List<Cat>()
            {
                new Cat
                {
                    id = "0XYvRd7oD",
                    Fan = user1,
                    width = 1204,
                    height = 1445,
                    url = "https://cdn2.thecatapi.com/images/0XYvRd7oD.jpg",
                    categories = new List<Category>
                    {
                        new Category
                        {
                            id = 1,
                            name = "boxes"
                        }
                    },
                    breeds = new List<Breed>
                    {
                        new Breed
                        {
                            id = "abys",
                            name = "Abyssinian",
                            cfa_url = "http://cfa.org/Breeds/BreedsAB/Abyssinian.aspx",
                            vetstreet_url = "http://www.vetstreet.com/cats/abyssinian",
                            temperament = "Active, Energetic, Independent, Intelligent, Gentle",
                            origin = "Egypt",
                            description = "The Abyssinian is easy to care for, and a joy to have in your home. They’re affectionate cats and love both people and other animals.",
                            wikipedia_url = "https://en.wikipedia.org/wiki/Abyssinian_(cat)"
                        }
                    }
                },
            };

            mock.Setup(m => m.GetAll()).Returns(() => cats);

            mock.Setup(m => m.GetById("0XYvRd7oD")).Returns(() => cats[0]);

            mock.Setup(m => m.Delete("0XYvRd7oD")).Callback(() => cats.RemoveAt(0));

            mock.Setup(m => m.GetByBreed("abys")).Returns(() => new List<Cat> { cats[0] });

            mock.Setup(m => m.AddToFavourites(catToAdd)).Callback<Cat>((cat) => cats.Add(cat));
            
            mock.Setup(m => m.GetFavouritesByUser(username)).Returns(() => new List<Cat> { cats[0] });

            mock.Setup(m => m.GetUser(username)).Returns(() => user1);

            return mock;
        }
    }

    [TestClass]
    public class DBTests
    {
        [TestMethod]
        public void TestGetAll()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            Assert.IsNotNull(store.GetAll());
            Assert.AreEqual(1, store.GetAll().Count());
        }

        [TestMethod]
        public void TestGetById()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            var cat = store.GetById("0XYvRd7oD");
            Assert.IsNotNull(cat);
            Assert.AreEqual(1, cat.breeds!.Count);
            Assert.AreEqual("abys", cat.breeds[0].id);
        }

        [TestMethod]
        public void TestGetByBreed()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            var cats = store.GetByBreed("abys");
            Assert.IsNotNull(cats);
            Assert.AreEqual(1, cats.Count);
        }

        [TestMethod]
        public void TestDelete()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            store.Delete("0XYvRd7oD");
            Assert.AreEqual(0, store.GetAll().Count);
        }

        [TestMethod]
        public void TestGetFavouritesByUser()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            var cats = store.GetFavouritesByUser("User1");
            Assert.IsNotNull(cats);
            Assert.AreEqual(1, cats.Count);
        }

        [TestMethod]
        public void TestAddToFavourites()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            store.AddToFavourites(MockStoreWrapper.catToAdd);
            Assert.AreEqual(2, store.GetAll().Count);
        }

        [TestMethod]
        public void TestGetUser()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            var user = store.GetUser("User1");
            Assert.IsNotNull(user);
            Assert.AreEqual("User1", user.UserName);
        }
    }

}
