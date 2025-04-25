builder.Services.AddSingleton<IAnimalRepository, AnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

builder.Services.AddScoped<IAnimalTransferService, AnimalTransferService>();
// Добавь сюда и другие сервисы, если надо
