using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaEco.Abstractions.Models.Customer;
using SeaEco.EntityFramework.Entities;
using SeaEco.Server.Controllers;
using SeaEco.Services.CustomerServices;

namespace SeaEco.ServerTests;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomerController     _ctrl;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _ctrl = new CustomerController(_customerServiceMock.Object);

            // For ModelState-validering på POST/PUT:
            _ctrl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        #region GetCustomerNames

        [Fact]
        public async Task GetCustomerNames_WhenNonEmpty_ReturnsOkWithList()
        {
            // Arrange
            var list = new List<CustomerNamesDto>
            {
                new CustomerNamesDto { Id = Guid.NewGuid(), CustomerName = "Seafood AS" }
            };
            _customerServiceMock
                .Setup(s => s.GetCustomerNames())
                .ReturnsAsync(list);

            // Act
            var result = await _ctrl.GetCustomerNames();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetCustomerNames_WhenEmpty_ReturnsNotFoundWithMessage()
        {
            // Arrange
            _customerServiceMock
                .Setup(s => s.GetCustomerNames())
                .ReturnsAsync(new List<CustomerNamesDto>());

            // Act
            var result = await _ctrl.GetCustomerNames();

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("No customers found");
        }

        #endregion

        #region GetCustomerInfoById

        [Fact]
        public async Task GetCustomerInfoById_WhenFound_ReturnsOkWithDto()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new CustomerDto
            {
                Id = id,
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Ola",
                Telefon       = "12345678"
            };
            _customerServiceMock
                .Setup(s => s.GetCustomerInfoById(id))
                .ReturnsAsync(dto);

            // Act
            var result = await _ctrl.GetCustomerInfoById(id);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(dto);
        }

        [Fact]
        public async Task GetCustomerInfoById_WhenNotFound_ReturnsNotFoundWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _customerServiceMock
                .Setup(s => s.GetCustomerInfoById(id))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _ctrl.GetCustomerInfoById(id);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be($"Customer with ID {id} not found");
        }

        #endregion

        #region GetAllProjectDetailsById

        [Fact]
        public async Task GetAllProjectDetailsById_WhenFound_ReturnsOkWithEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var kunde = new Kunde
            {
                Id = id,
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Ola",
                Telefon       = "12345678"
            };
            _customerServiceMock
                .Setup(s => s.GetAllProjectDetailsById(id))
                .ReturnsAsync(kunde);

            // Act
            var result = await _ctrl.GetAllProjectDetailsById(id);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(kunde);
        }

        [Fact]
        public async Task GetAllProjectDetailsById_WhenNotFound_ReturnsNotFoundWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _customerServiceMock
                .Setup(s => s.GetAllProjectDetailsById(id))
                .ReturnsAsync((Kunde?)null);

            // Act
            var result = await _ctrl.GetAllProjectDetailsById(id);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be($"Customer with ID {id} not found");
        }

        #endregion

        #region AddCustomer

        [Fact]
        public async Task AddCustomer_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Oppdragsgiver", "Påkrevd");
            var dto = new EditCustomerDto();

            // Act
            var result = await _ctrl.AddCustomer(dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task AddCustomer_WhenSuccess_ReturnsOkWithMessage()
        {
            // Arrange
            var dto = new EditCustomerDto
            {
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Ola",
                Telefon       = "12345678"
            };
            _customerServiceMock
                .Setup(s => s.AddCustomer(dto))
                .ReturnsAsync(new EditCustomerResult { IsSuccess = true, Message = "OK" });

            // Act
            var result = await _ctrl.AddCustomer(dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("OK");
        }

        [Fact]
        public async Task AddCustomer_WhenFailure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var dto = new EditCustomerDto
            {
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Ola",
                Telefon       = "12345678"
            };
            _customerServiceMock
                .Setup(s => s.AddCustomer(dto))
                .ReturnsAsync(new EditCustomerResult { IsSuccess = false, Message = "Feil" });

            // Act
            var result = await _ctrl.AddCustomer(dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        #endregion

        #region UpdateCustomer

        [Fact]
        public async Task UpdateCustomer_WhenModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Telefon", "Ugyldig");
            var dto = new EditCustomerDto();

            // Act
            var result = await _ctrl.UpdateCustomer(Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenSuccess_ReturnsOkWithMessage()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditCustomerDto
            {
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Ola",
                Telefon       = "12345678"
            };
            _customerServiceMock
                .Setup(s => s.UpdateCustomer(dto, id))
                .ReturnsAsync(new EditCustomerResult { IsSuccess = true, Message = "Oppdatert" });

            // Act
            var result = await _ctrl.UpdateCustomer(id, dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("Oppdatert");
        }

        [Fact]
        public async Task UpdateCustomer_WhenFailure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditCustomerDto
            {
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Ola",
                Telefon       = "12345678"
            };
            _customerServiceMock
                .Setup(s => s.UpdateCustomer(dto, id))
                .ReturnsAsync(new EditCustomerResult { IsSuccess = false, Message = "Feil" });

            // Act
            var result = await _ctrl.UpdateCustomer(id, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        #endregion

        #region DeleteCustomer

        [Fact]
        public async Task DeleteCustomer_WhenSuccess_ReturnsOkWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _customerServiceMock
                .Setup(s => s.DeleteCustomer(id))
                .ReturnsAsync(new EditCustomerResult { IsSuccess = true, Message = "Slettet" });

            // Act
            var result = await _ctrl.DeleteCustomer(id);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("Slettet");
        }

        [Fact]
        public async Task DeleteCustomer_WhenFailure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _customerServiceMock
                .Setup(s => s.DeleteCustomer(id))
                .ReturnsAsync(new EditCustomerResult { IsSuccess = false, Message = "Feil" });

            // Act
            var result = await _ctrl.DeleteCustomer(id);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        #endregion
}