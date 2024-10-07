using System.Text.Json;

namespace DynamicObjectCreation.Application.Extensions
{
	public static class JsonElementExtension
	{
		public static bool TryFindValueByCaseIgnoredKey(this JsonElement element, string key, out JsonElement value)
		{
			if (element.ValueKind == JsonValueKind.Object)
			{
				foreach (var property in element.EnumerateObject())
				{
					if (string.Equals(property.Name, key, StringComparison.OrdinalIgnoreCase))
					{
						value = property.Value;
						return true;
					}
				}

				foreach (var property in element.EnumerateObject())
				{
					if (property.Value.ValueKind == JsonValueKind.Object || property.Value.ValueKind == JsonValueKind.Array)
					{
						if (TryFindValueByCaseIgnoredKey(property.Value, key, out value))
						{
							return true;
						}
					}
				}
			}
			else if (element.ValueKind == JsonValueKind.Array)
			{
				foreach (var arrayElement in element.EnumerateArray())
				{
					if (arrayElement.ValueKind == JsonValueKind.Object || arrayElement.ValueKind == JsonValueKind.Array)
					{
						if (TryFindValueByCaseIgnoredKey(arrayElement, key, out value))
						{
							return true;
						}
					}
				}
			}

			value = default;
			return false;
		}
	}
}
