﻿using System;

using NUnit.Framework;
using NUnit.Tests.Models.Default;
using Xamarin.Forms.Mocks;
using System.Linq;

using Game.Controllers;
using Game.Models;
using Game.GameEngine;
using Game.ViewModels;
using Game.Services;
using Game.Views;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace NUnit.Tests.Models
{
    [TestFixture]
    class ScoresViewModelTests
    {
        #region ScoresViewModelBasics
        [Test]
        public void ViewModel_ScoresViewModel_Instantiate_Should_Pass()
        {

            MockForms.Init();

            var Actual = new ScoresViewModel();

            // Validate the controller can stand up and has a Title
            Assert.AreEqual("Score List", Actual.Title, TestContext.CurrentContext.Test.Name);
        }

        #endregion ScoresViewModelBasics

        #region DataOperations
        [Test]
        public async Task ViewModel_ScoresViewModel_AddData_Should_Pass()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            var myData = DefaultModels.ScoreDefault();
            var myReturn = await myViewModel.AddAsync(myData);

            var Actual = await myViewModel.GetAsync(myData.Id);
            var Expected = myData;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected.Id, Actual.Id, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public async Task ViewModel_ScoresViewModel_DeleteData_Should_Pass()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            var myData = DefaultModels.ScoreDefault();
            await myViewModel.AddAsync(myData);

            var myReturn = await myViewModel.DeleteAsync(myData);

            var Actual = await myViewModel.GetAsync(myData.Id);
            Object Expected = null;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public async Task ViewModel_ScoresViewModel_UpdateData_Should_Pass()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            var myData = DefaultModels.ScoreDefault();
            await myViewModel.AddAsync(myData);

            var value = "new";

            myData.Name = value;
            var myReturn = myViewModel.UpdateAsync(myData);

            var Actual = await myViewModel.GetAsync(myData.Id);
            string Expected = value;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual.Name, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public async Task ViewModel_ScoresViewModel_UpdateData_Bogus_Should_Skip()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            // Load data
            var canExecute = myViewModel.LoadDataCommand.CanExecute(null);
            myViewModel.LoadDataCommand.Execute(null);

            var myData = DefaultModels.ScoreDefault();

            // Make the ID bogus...
            var value = "new";
            myData.Id = value;

            var myReturn = await myViewModel.UpdateAsync(myData);

            var Actual = myReturn;
            bool Expected = false;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
        #endregion DataOperations

        #region MessageCenter
        [Test]
        public async Task ViewModel_MessageCenter_ScoresViewModel_MessageCenter_AddData_Should_Pass()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            var myData = DefaultModels.ScoreDefault();

            var myPage = new NewScorePage();
            MessagingCenter.Send(myPage, "AddData", myData);

            var Actual = await myViewModel.GetAsync(myData.Id);
            var Expected = myData;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected.Id, Actual.Id, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public async Task ViewModel_MessageCenter_ScoresViewModel_MessageCenter_DeleteData_Should_Pass()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            var myData = DefaultModels.ScoreDefault();
            await myViewModel.AddAsync(myData);

            var myPage = new DeleteScorePage(new ScoreDetailViewModel(new Score()));
            MessagingCenter.Send(myPage, "DeleteData", myData);

            var Actual = await myViewModel.GetAsync(myData.Id);
            Object Expected = null;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public async Task ViewModel_MessageCenter_ScoresViewModel_MessageCenter_UpdateData_Should_Pass()
        {

            MockForms.Init();

            // Get State of the DataStore, and set to run on the Mock
            var myDataStoreEnum = MasterDataStore.GetDataStoreMockFlag();
            MasterDataStore.ToggleDataStore(DataStoreEnum.Mock);

            var myViewModel = new ScoresViewModel();
            var myData = DefaultModels.ScoreDefault();
            await myViewModel.AddAsync(myData);

            var value = "new";

            myData.Name = value;

            var myPage = new EditScorePage(new ScoreDetailViewModel(new Score()));
            MessagingCenter.Send(myPage, "EditData", myData);

            var Actual = await myViewModel.GetAsync(myData.Id);
            string Expected = value;

            // Return state
            MasterDataStore.ToggleDataStore(myDataStoreEnum);

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual.Name, TestContext.CurrentContext.Test.Name);
        }

        #endregion MessageCenter

        #region LoadRefesh
        [Test]
        public void ViewModel_ScoresViewModel_SetNeedsRefresh_False_Should_Be_False()
        {

            MockForms.Init();

            var myData = new ScoresViewModel();

            myData.SetNeedsRefresh(false);

            var Actual = myData.NeedsRefresh();
            var Expected = false;

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ViewModel_ScoresViewModel_SetNeedsRefresh_True_Should_Be_True()
        {

            MockForms.Init();

            var myData = new ScoresViewModel();

            myData.SetNeedsRefresh(true);

            var Actual = myData.NeedsRefresh();
            var Expected = true;

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ViewModel_ScoresViewModel_DataSet_Should_Be_Valid()
        {

            MockForms.Init();

            var myData = new ScoresViewModel();
            var myLoad = myData.LoadDataCommand;

            var Actual = myData.Dataset;
            var Expected = myData.Dataset;

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ViewModel_ScoresViewModel_LoadDataCommand_Should_Pass()
        {

            MockForms.Init();

            var myData = new ScoresViewModel();

            // Load data
            var canExecute = myData.LoadDataCommand.CanExecute(null);
            myData.LoadDataCommand.Execute(null);

            var Actual = myData.Dataset.Count();
            var Expected = myData.DataStore.GetAllAsync_Score().GetAwaiter().GetResult().Count();

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ViewModel_ScoresViewModel_LoadDataCommand_With_IsBusy_Should_Skip()
        {

            MockForms.Init();

            var myData = new ScoresViewModel();
            var myIsBusy = myData.IsBusy;
            myData.IsBusy = true;

            var canExecute = myData.LoadDataCommand.CanExecute(null);
            myData.LoadDataCommand.Execute(null);

            var Actual = myData.Dataset.Count();
            var Expected = 0;

            // set it back...
            myData.IsBusy = myIsBusy;

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ViewModel_ScoresViewModel_LoadCommand_With_Bogus_DataSource_Should_Throw_Skip()
        {

            MockForms.Init();

            var myData = new ScoresViewModel();
            var myIsBusy = myData.IsBusy;

            // Make the data store null, this will fire the Exception, which then skips...
            myData.DataStore = null;

            var canExecute = myData.LoadDataCommand.CanExecute(null);
            myData.LoadDataCommand.Execute(null);

            var Actual = myData.Dataset.Count();
            var Expected = 0;

            // Validate the controller can stand up and has a Title
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        #endregion LoadRefesh

    }
}



