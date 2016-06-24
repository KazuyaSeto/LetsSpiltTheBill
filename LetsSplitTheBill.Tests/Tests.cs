using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace LetsSplitTheBill.Tests
{
	//[TestFixture(Platform.Android)]
	//[TestFixture(Platform.iOS)]
	[TestFixture]
	public class Tests
	{

		//IApp app;
		//Platform platform;
		CheckModel model = null;
		//public Tests(Platform platform)
		public Tests()
		{
			//this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			//app = AppInitializer.StartApp(platform);
			model = new CheckModel();
		}

		[Test]
		public void AppLaunches()
		{
			//app.Screenshot("First screen.");
			Assert.True(true, "TestSample ok");


		}

		[Test]
		public void CheckModelRegistMenuTest()
		{
			Assert.True(model.GetMenusCount() == 0);

			model.RegistMenu("サラダ",700);
			Assert.True(model.GetMenusCount() == 1);
			Assert.True(model.GetMenu("サラダ").name == "サラダ");
			Assert.True(model.GetMenu("サラダ").price == 700);

			model.RegistMenu("パテ", 1000);
			Assert.True(model.GetMenusCount() == 2);
			Assert.True(model.GetMenu("パテ").name == "パテ");
			Assert.True(model.GetMenu("パテ").price == 1000);

			model.Clear();

			Assert.True(model.GetMenusCount() == 0);
			Assert.True(model.GetMenu("パテ") == null);

			model.Clear();
		}

		[Test]
		public void CheckModelRegistParticipantTest()
		{
			Assert.True(model.GetParticipantsCount() == 0);

			model.RegistParticipant("ryu");
			Assert.True(model.GetParticipantsCount() == 1);
			Assert.True(model.GetParticipant("ryu").name == "ryu");

			model.RegistParticipant("yuko");
			Assert.True(model.GetParticipantsCount() == 2);
			Assert.True(model.GetParticipant("yuko").name == "yuko");

			model.Clear();

			Assert.True(model.GetParticipantsCount() == 0);
			Assert.True(model.GetParticipant("yuko") == null);

			model.Clear();
		}

		[Test]
		public void CheckSumTest()
		{
			model.RegistMenu("サラダ",1);
			model.RegistMenu("ハンバーグ",10);
			model.RegistMenu("ポテト",100);
			model.RegistMenu("スープ",1000);

			model.AddFood("サラダ");
			Assert.True(model.GetTotalPrice() == 1);
			model.AddFood("ハンバーグ");
			Assert.True(model.GetTotalPrice() == 11);
			model.AddFood("ポテト");
			Assert.True(model.GetTotalPrice() == 111);
			model.AddFood("スープ");
			Assert.True(model.GetTotalPrice() == 1111);

			model.Clear();
		}

		[Test]
		public void GetParticipantTotalPriceTest()
		{
			model.RegistParticipant("ryu");
			model.RegistParticipant("yuko");
			model.RegistParticipant("haru");

			model.RegistMenu("サラダ", 3);
			model.RegistMenu("ハンバーグ", 20);
			model.RegistMenu("ポテト", 500);
			model.RegistMenu("スープ", 3000);

			model.AddFood("サラダ");
			var salad = model.GetFood(0);
			salad.AddEatar(model.GetParticipant("ryu"));
			salad.AddEatar(model.GetParticipant("yuko"));
			salad.AddEatar(model.GetParticipant("haru"));

			model.AddFood("ハンバーグ");
			var hanbagu = model.GetFood(1);
			hanbagu.AddEatar(model.GetParticipant("ryu"));
			hanbagu.AddEatar(model.GetParticipant("yuko"));

			model.AddFood("ポテト");
			var poteto = model.GetFood(2);
			poteto.AddEatar(model.GetParticipant("haru"));
			poteto.AddEatar(model.GetParticipant("haru"));
			poteto.AddEatar(model.GetParticipant("yuko"));
			poteto.AddEatar(model.GetParticipant("ryu"));
			poteto.AddEatar(model.GetParticipant("yuko"));

			model.AddFood("スープ");
			var soup = model.GetFood(3);
			soup.AddEatar(model.GetParticipant("haru"));
			soup.AddEatar(model.GetParticipant("yuko"));
			soup.AddEatar(model.GetParticipant("ryu"));

			Assert.True((int)model.GetParticipantTotalPrice("ryu") == 1111, model.GetParticipantTotalPrice("ryu").ToString() );
			Assert.True((int)model.GetParticipantTotalPrice("yuko") == 1211, model.GetParticipantTotalPrice("yuko").ToString());
			Assert.True((int)model.GetParticipantTotalPrice("haru") == 1201, model.GetParticipantTotalPrice("haru").ToString());
		}
	}
}

