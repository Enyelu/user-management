using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;
using user_management.core.Commands.Onboarding;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.domain.Models;
using user_management.infrastructure;
using user_management.infrastructure.Services.Interfaces;
using user_management.infrastructure.Shared;

public class HandleSignUpTests
{
    private readonly IFixture _fixture;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ILogger<HandleSignUp.Handler>> _loggerMock;
    private readonly Mock<IMailService> _mailServiceMock;
    private readonly ApplicationContext _dbContext;
    private readonly IOptions<Settings> _options;

    public HandleSignUpTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<HandleSignUp.Handler>>>();
        _mailServiceMock = _fixture.Freeze<Mock<IMailService>>();

        // Setup UserManager mock
        var store = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);

        // Setup InMemory DbContext
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationContext(options);

        // Setup IOptions<Settings>
        var settings = _fixture.Create<Settings>();
        settings.CipherKeyIvPhrase = "PuRhtPnSTj7Brqv9|NmtoCTHuHFT7iBXU";
        _options = Options.Create(settings);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = _fixture.Build<HandleSignUp.Command>()
                              .With(c => c.Password, Convert.ToBase64String(Encoding.UTF8.GetBytes("password")))
                              .Create();
        command.TenantId = null;
        command.IsTenantStaff = false;
        command.Password = EncryptionHelper.Encrypt("Password@123", "PuRhtPnSTj7Brqv9|NmtoCTHuHFT7iBXU");

        var roleId = await _dbContext.Roles.AddAsync(new ApplicationRole { Id = command .RoleId, Name = "Test"});
        await _dbContext.SaveChangesAsync();

        var handler = new HandleSignUp.Handler(_dbContext, _fixture.Create<IMapper>(), _userManagerMock.Object, _options, _mailServiceMock.Object, _loggerMock.Object);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("emailToken");

        _mailServiceMock.Setup(x => x.SendMailAsync(It.IsAny<EmailRequest>()))
            .ReturnsAsync(true);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Message.Should().Be("Registration was successful, please check your email to complete process.");
    }

    [Fact]
    public async Task Handle_WithInvalidRole_ShouldReturnFailureResponse()
    {
        // Arrange
        var command = _fixture.Create<HandleSignUp.Command>();
        var handler = new HandleSignUp.Handler(_dbContext, _fixture.Create<IMapper>(), _userManagerMock.Object, _options, _mailServiceMock.Object, _loggerMock.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Message.Should().Contain("TenantId cannot be null when user is tennant staff. Please enter correct value and retry...");
    }
}