namespace Medical.Application.Helpers;

public static class UserMappingHelper
{
    public static UserResponse MapFromDto(UserDto dto) => new()
    {
        Id = dto.Id,
        UserName = dto.UserName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        Role = dto.Role,
        OrganizationId = dto.OrganizationId
    };

    public static UserResponse MapFromAnonymous(object data)
    {
        if (data is UserDto dto)
            return MapFromDto(dto);

        var json = JsonSerializer.Serialize(data);
        var parsed = JsonSerializer.Deserialize<UserDto>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return MapFromDto(parsed!);
    }

    public static IList<UserResponse> MapListFromAnonymous(object data)
    {
        if (data is IEnumerable<UserDto> dtos)
            return dtos.Select(MapFromDto).ToList();

        var json = JsonSerializer.Serialize(data);
        var list = JsonSerializer.Deserialize<List<UserDto>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
        return list.Select(MapFromDto).ToList();
    }
}
