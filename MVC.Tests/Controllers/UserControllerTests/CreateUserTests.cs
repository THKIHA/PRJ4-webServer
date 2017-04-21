﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Common.Models;
using MVC.Controllers;
using MVC.Identity;
using MVC.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace MVC.Tests.Controllers.UserControllerTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class CreateUserTests : BaseRepositoryTest
    {
        private UserController uut;
        private CreateUserViewModel viewModel;

        private IStore store;
        private IUserContext context;

        [SetUp]
        public void Setup()
        {
            context = Substitute.For<IUserContext>();
            store = Substitute.For<IStore>();

            uut = new UserController(Factory, context, store);
            uut.ControllerContext = new ControllerContext();

            viewModel = new CreateUserViewModel()
            {
                FirstName = "firstName",
                Email = "a@a.a",
                LastName = "lastName",
                Password1 = "1234",
                Password2 = "1234",
                UserName = "userName"
            };
        }

        [Test]
        public void Create_NoEmail_ReturnsError()
        {
            // Setup.
            viewModel.Email = "";

            // Act.
            var result = uut.Create(viewModel);

            // Assert the the view is correct.
            CheckViewName(result, "~/Views/Home/Index.cshtml");

            // Check that the viewmodel is correct.
            var model = CheckViewModel<CreateUserViewModel>(result);

            // Check that the viewmodel contains the correct data.
            Assert.That(model, Is.EqualTo(viewModel));

            // Check that the correct error is attached.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorEmailRequired);
        }

        [Test]
        public void Create_NoFirstName_ReturnsError()
        {
            // Setup.
            viewModel.FirstName = "";

            // Act.
            var result = uut.Create(viewModel);

            // Assert the the view is correct.
            CheckViewName(result, "~/Views/Home/Index.cshtml");

            // Check that the viewmodel is correct.
            var model = CheckViewModel<CreateUserViewModel>(result);

            // Check that the viewmodel contains the correct data.
            Assert.That(model, Is.EqualTo(viewModel));

            // Check that the correct error is attached.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorFirstNameRequired);
        }

        [Test]
        public void Create_NoLastName_ReturnsError()
        {
            // Setup.
            viewModel.LastName = "";

            // Act.
            var result = uut.Create(viewModel);

            // Assert the the view is correct.
            CheckViewName(result, "~/Views/Home/Index.cshtml");

            // Check that the viewmodel is correct.
            var model = CheckViewModel<CreateUserViewModel>(result);

            // Check that the viewmodel contains the correct data.
            Assert.That(model, Is.EqualTo(viewModel));

            // Check that the correct error is attached.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorLastNameRequired);
        }

        [Test]
        public void Create_NoPassword1_ReturnsError()
        {
            // Setup.
            viewModel.Password1 = "";

            // Act.
            var result = uut.Create(viewModel);

            // Assert the the view is correct.
            CheckViewName(result, "~/Views/Home/Index.cshtml");

            // Check that the viewmodel is correct.
            var model = CheckViewModel<CreateUserViewModel>(result);

            // Check that the viewmodel contains the correct data.
            Assert.That(model, Is.EqualTo(viewModel));

            // Check that the correct error is attached.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorPassword1Required);
        }

        [Test]
        public void Create_NoPassword2_ReturnsError()
        {
            // Setup.
            viewModel.Password2 = "";

            // Act.
            var result = uut.Create(viewModel);

            // Assert the the view is correct.
            CheckViewName(result, "~/Views/Home/Index.cshtml");

            // Check that the viewmodel is correct.
            var model = CheckViewModel<CreateUserViewModel>(result);

            // Check that the viewmodel contains the correct data.
            Assert.That(model, Is.EqualTo(viewModel));

            // Check that the correct error is attached.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorPassword2Required);
        }

        [Test]
        public void Create_NoUserName_ReturnsError()
        {
            // Setup.
            viewModel.UserName = "";

            // Act.
            var result = uut.Create(viewModel);

            // Assert the the view is correct.
            CheckViewName(result, "~/Views/Home/Index.cshtml");

            // Check that the viewmodel is correct.
            var model = CheckViewModel<CreateUserViewModel>(result);

            // Check that the viewmodel contains the correct data.
            Assert.That(model, Is.EqualTo(viewModel));

            // Check that the correct error is attached.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorUserNameRequired);
        }

        [Test]
        public void Create_UserAlreadyExists_ReturnsError()
        {
            var user = new User()
            {
                Username = viewModel.UserName
            };

            UserRepository.Get(Arg.Is(viewModel.UserName)).Returns(user);

            // Attempt to create the user.
            uut.Create(viewModel);

            // Assert that the error is present.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorUserNameTaken);
        }

        [Test]
        public void Create_EmailIsInUse_ReturnsError()
        {
            var user = new User()
            {
                Email = viewModel.Email
            };
            
            UserRepository.GetByEmail(Arg.Is(viewModel.UserName)).Returns(user);

            // Attempt to create the user.
            uut.Create(viewModel);

            // Assert that the error is present.
            CheckErrorOnModel(uut.ModelState, Resources.User.ErrorEmailInUse);
        }

        [Test]
        public void Create_CallsRepository()
        {
            // Setup.
            UserRepository.DidNotReceive().Add(Arg.Any<User>());
            MyWork.DidNotReceive().Complete();

            // Act.
            uut.Create(viewModel);

            // Assert.
            UserRepository.Received(1).Add(Arg.Any<User>());
            MyWork.Received(1).Complete();
        }

        [Test]
        public void Create_WithValidData_ReturnsFrontPage()
        {
            // Act.
            var result = uut.Create(viewModel);

            // Assert.
            CheckRedirectsToRouteWithId(result, "Index");
        }
    }
}
