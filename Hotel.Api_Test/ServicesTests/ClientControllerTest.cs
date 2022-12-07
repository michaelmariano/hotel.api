using Application.Classes;
using Application.Interfaces;
using Bogus;
using Bogus.Extensions.Canada;
using Bogus.Extensions.UnitedStates;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.Models;
using FluentAssertions;
using Hotel.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Repository.Interfaces;

namespace Hotel.Api_Test.ServicesTests
{
    public class ClientControllerTest
    {
        private readonly ClientController _clientController;
        private readonly IClientApp _clientApp;
        private readonly IClientRepository _clientRepository;
        private readonly Faker _faker;

        public ClientControllerTest()
        {
            _faker = new Faker();

            _clientRepository = Substitute.For<IClientRepository>();

            _clientApp = new ClientApp(_clientRepository);

            _clientController = new ClientController(_clientApp);
        }

        [Fact(DisplayName = "InsertClientShouldReturnOkResultAndIdValue")]
        public async Task InsertClientShouldReturnOkResultAndIdValue()
        {
            //Arrange            
            int idClient = _faker.Random.Int(1, 1000);

            var client = new Client
            {
                IdClient = idClient,
                Document = _faker.Person.Sin(),
                Email = _faker.Person.Email,
                FullName = _faker.Person.FullName,
                PhoneNumber = _faker.Person.Phone,
                Password = "Test123!"
            };

            _clientRepository.InsertAsync(client).Returns(idClient);

            //Act
            var result = await _clientController.InsertAsync(client);

            //Assert
            result.Should().NotBeNull();

            var objectResult = result as CreatedResult;

            objectResult.Should().BeOfType<CreatedResult>();

            var objResult = result as ObjectResult;

            var idClientCurrent = Convert.ToInt32(objResult?.Value);

            Assert.Equal(client.IdClient, idClientCurrent);
        }

        [Fact(DisplayName = "LoginShouldReturnOkResultAndIdValue")]
        public async Task LoginShouldReturnOkResultAndIdValue()
        {
            //Arrange            
            int idClient = _faker.Random.Int(1, 1000);
            string email = _faker.Person.Email;
            string password = "Test123!";

            var login = new LoginModel
            {
                Email = email,
                Password = password
            };

            var client = new Client
            {
                IdClient = idClient,
                Document = _faker.Person.Sin(),
                Email = email,
                FullName = _faker.Person.FullName,
                PhoneNumber = _faker.Person.Phone,
                Password = password.EncryptPasswordWithSHA256()
            };

            _clientRepository.GetByEmailAsync(login.Email).Returns(client);

            //Act
            var result = await _clientController.LoginAsync(login);

            //Assert
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;

            objectResult.Should().BeOfType<OkObjectResult>();

            var objResult = result as ObjectResult;

            var clientCurrent = objResult?.Value as Client;

            Assert.Equal(client, clientCurrent);
        }

        [Fact(DisplayName = "LoginShouldReturnUnauthorizedIfEmailIsNotFounded")]
        public async Task LoginShouldReturnUnauthorizedIfEmailIsNotFounded()
        {
            //Arrange      
            string email = _faker.Person.Email;
            string password = "Test123!";
            int expectedStatusCode = 401;
            string expectedMessage = "Access denied.";
            int currentStatusCode = 0;
            string currentMessage = "";

            var login = new LoginModel
            {
                Email = email,
                Password = password
            };

            Client client = null;

            _clientRepository.GetByEmailAsync(login.Email).Returns(client);

            //Act
            IActionResult? result;

            try
            {
                result = await _clientController.LoginAsync(login);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Fact(DisplayName = "LoginShouldReturnUnauthorizedIfPasswordIsWrong")]
        public async Task LoginShouldReturnUnauthorizedIfPasswordIsWrong()
        {
            //Arrange            
            int idClient = _faker.Random.Int(1, 1000);
            string email = _faker.Person.Email;
            string password = "Test123!";
            int expectedStatusCode = 401;
            string expectedMessage = "Access denied.";
            int currentStatusCode = 0;
            string currentMessage = "";

            var login = new LoginModel
            {
                Email = email,
                Password = "1daHsDd84823e24f"
            };

            var client = new Client
            {
                IdClient = idClient,
                Document = _faker.Person.Sin(),
                Email = email,
                FullName = _faker.Person.FullName,
                PhoneNumber = _faker.Person.Phone,
                Password = password.EncryptPasswordWithSHA256()
            };

            _clientRepository.GetByEmailAsync(login.Email).Returns(client);

            //Act
            IActionResult? result;

            try
            {
                result = await _clientController.LoginAsync(login);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }
    }
}
