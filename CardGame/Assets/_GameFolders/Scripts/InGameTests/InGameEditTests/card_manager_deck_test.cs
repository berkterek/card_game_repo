using System.Linq;
using CardGame.Abstracts.DataContainers;
using CardGame.Managers;
using NSubstitute;
using NUnit.Framework;

namespace Card_Manager_Tests
{
    public class card_manager_deck_test
    {
        DeckValue _deckValue;
        
        void SetUp(int count)
        {
            _deckValue = new DeckValue();

            ICardDataContainer[] cardDataContainers = new ICardDataContainer[count]; 
            for (int i = 0; i < count; i++)
            {
                cardDataContainers[i] = Substitute.For<ICardDataContainer>();
            }

            _deckValue.CardDataContainers = cardDataContainers;
        }

        [Test]
        public void get_card_data_containers_returns_no_more_than_available()
        {
            int count = 10;
            SetUp(count);
            var result = _deckValue.GetCardDataContainers(count);
            Assert.AreEqual(_deckValue.CardDataContainers.Length, result.Length);
        }

        [Test]
        public void get_card_data_containers_with_zero_count_returns_empty_array()
        {
            int count = 0;
            SetUp(count);
            var result = _deckValue.GetCardDataContainers(count);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void get_card_data_containers_with_exact_count_returns_exact()
        {
            int count = 10;
            SetUp(count);
            var result = _deckValue.GetCardDataContainers(_deckValue.CardDataContainers.Length);
            Assert.AreEqual(_deckValue.CardDataContainers.Length, result.Length);
        }

        [Test]
        public void get_card_data_containers_returns_all_unique_containers()
        {
            int count = 10;
            SetUp(count);
            var result = _deckValue.GetCardDataContainers(count);
            Assert.AreEqual(result.Distinct().Count(), result.Length);
        }
        
        [Test]
        public void get_card_data_containers_with_half_count_returns_exact()
        {
            int count = 10;
            int resultCount = count / 2;
            SetUp(count);
            var result = _deckValue.GetCardDataContainers(resultCount);
            Assert.AreEqual(resultCount, result.Length);
        }
    }    
}
