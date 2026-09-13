namespace Medical.Application.Extensions;
public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register MediatR with pipeline behaviors
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register repositories with Scoped lifetime
        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        
        // Register Inventory module repositories
        services.AddScoped<IMedicineBatchRepository, MedicineBatchRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IBillInventoryService, BillInventoryService>();
        services.AddScoped<IBillService, BillService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Register AI services
        services.AddScoped<IAIService, AIService>();

        // Register export service
        services.AddScoped<ExportService>();

        // Register infrastructure services (DbContext, Identity, etc.)
        services.AddInfraService(configuration);
        return services;
    }
}
