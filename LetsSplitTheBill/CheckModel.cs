using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LetsSplitTheBill
{
	/// <summary>
	/// Check model.
	/// 会計用モデル
	/// </summary>
	public class CheckModel
	{
		/// <summary>
		/// Participant.
		/// 参加者
		/// </summary>
		public class Participant
		{
			public string name { get; private set; }
			public Participant(string name)
			{
				Init(name);
			}
			public void Init(string name)
			{
				this.name = name;
			}

			public Eater CreateEater()
			{
				var eater = new Eater(this);
				return eater;
			}
		}

		/// <summary>
		/// Eatar.
		/// 食べた人を表すクラス
		/// </summary>
		public class Eater
		{
			public Participant participant { get; private set; }
			public int count { get; set; }
			public Eater(Participant participant)
			{
				this.participant = participant;
				count = 1;
			}
		}

		public class Menu
		{
			public string name { get; private set; }
			public int price { get; private set; }

			public Menu(string name, int price)
			{
				Init(name,price);
			}

			public void Init(string name, int price)
			{
				this.name = name;
				this.price = price;
			}

			public Food CreateFood()
			{
				return new Food(this);
			}
		}

		/// <summary>
		/// Food.
		/// 注文を表すクラス
		/// </summary>
		public class Food
		{
			public int id { get; private set; }
			public Menu menu;
			Dictionary<string, Eater> eatarMap = new Dictionary<string, Eater>();

			public Food(Menu menu)
			{
				this.menu = menu;
			}

			public void AddEatar(Participant participant)
			{
				if (eatarMap.ContainsKey(participant.name))
				{
					eatarMap[participant.name].count++;
				}
				else
				{
					eatarMap.Add(participant.name, participant.CreateEater());
				}
			}

			public void RemoveEatar(Participant participant)
			{
				if (eatarMap.ContainsKey(participant.name))
				{
					eatarMap[participant.name].count--;
				}

				if (eatarMap[participant.name].count <= 0)
				{
					eatarMap.Remove(participant.name);
				}
			}

			public float GetSplitedPrice(Participant participant)
			{
				if (!eatarMap.ContainsKey(participant.name))
				{
					return 0;
				}

				int count = 0;

				foreach (var item in eatarMap)
				{
					count += item.Value.count;
				}

				return ( menu.price / count ) * eatarMap[participant.name].count;
			}
		}

		Dictionary<string, Menu> menuDictionary = new Dictionary<string, Menu>();
		List<Food> foodList = new List<Food>();
		Dictionary<string, Participant> participantDictionary = new Dictionary<string, Participant>();

		public CheckModel()
		{
		}

		public void Clear()
		{
			menuDictionary.Clear();
			participantDictionary.Clear();
			foodList.Clear();
		}

		public void RegistMenu(string name, int price)
		{
			if (menuDictionary.ContainsKey(name))
			{
				menuDictionary[name].Init(name,price);
			}
			menuDictionary[name] = new Menu(name, price);
		}

		public Menu GetMenu(string name)
		{
			if (!menuDictionary.ContainsKey(name)) return null;
			return menuDictionary[name];
		}

		public int GetMenusCount()
		{
			return menuDictionary.Count;
		}

		public void RegistParticipant(string name)
		{
			if (participantDictionary.ContainsKey(name))
			{
				participantDictionary[name].Init(name);
			}
			participantDictionary[name] = new Participant(name);
		}

		public Participant GetParticipant(string name)
		{
			if (!participantDictionary.ContainsKey(name)) return null;
			return participantDictionary[name];
		}

		public int GetParticipantsCount()
		{
			return participantDictionary.Count;
		}

		public bool AddFood(string menuName)
		{
			if (!menuDictionary.ContainsKey(menuName)) return false;
			foodList.Add(menuDictionary[menuName].CreateFood());
			return true;
		}

		public Food GetFood(int index)
		{
			if (index >= foodList.Count) return null;
			return foodList[index];
		}

		public int GetTotalPrice()
		{
			return foodList.Select(food => food.menu.price).Sum();
		}

		public float GetParticipantTotalPrice(string participantName)
		{
			if (!participantDictionary.ContainsKey(participantName)) return 0.0f;
			var participant = participantDictionary[participantName];
			return foodList.Select(food => food.GetSplitedPrice(participant)).Sum();
		}

		public static void test()
		{
			System.Diagnostics.Debug.WriteLine("test start");
		}
	}
}

