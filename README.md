# DynamicObjectCreation

This project demonstrates the dynamic creation and management of objects and their transactions. It provides a flexible and scalable approach to handling dynamic data structures and applies validation rules accordingly.

Please feel free to clone repository and enjoy as it yours.

Here are the steps to get the project run on your machine:

1. **Clone the repository**
2. **Setup Database:**
    - After cloning the project, you'll need to set up your database. Ensure you have PostgreSQL installed and configured.
3. **Apply Migrations:**
    - The project uses Entity Framework Core for data handling. Run the following commands to create the database and apply the necessary migrations:
    `dotnet ef database update`
4. **Create Required PostgreSQL Function:**
    - For the application to work as intended, you need to create a custom function in your PostgreSQL database. This function allows for deep search operations in JSONB fields. Run the following SQL command after     setting up the database:
      
      ```pgsql
      CREATE OR REPLACE FUNCTION jsonb_deep_search(json_data jsonb, key text, value text, is_string_value boolean)
      RETURNS boolean AS $$
      DECLARE
          path_query text;
      BEGIN
          IF is_string_value = TRUE THEN
              path_query := format('$.** ? (@.%s == "%s")', lower(key), lower(value));
          ELSE
              path_query := format('$.** ? (@.%s == %s)', lower(key), lower(value));
          END IF;
      
          RETURN jsonb_path_exists(lower(json_data::text)::jsonb, path_query::jsonpath);
      END;
      $$ LANGUAGE plpgsql;
      ```
      

Once the database is ready, you're good to go!


