# Database Connectivity (ODBC and JDBC)

## Classroom Preparation

## Problem Statement
A common requirement for most of the applications that we use is that they need to maintain "persistent state".  This means that certain interactions with the application have lasting effects that can be recalled hours, days, or weeks later.

- Order history at Amazon.com
- LinkedIn profile information
- Email messages in GMail

This data often needs to be searched and updated in order for the application to fulfill it's purpose. One of the most common ways an application stores persistent data is by using a database.

We've already seen how we can interact with a database directly by typing SQL commands into a GUI client (i.e. Visual Studio or PGAdmin), today we'll learn how to write application code that can interact with a database in order to read and write persistent data.

## Objectives

* Review Database Design Exercise
* Making Connections
* Executing SQL Statements 
* SQL Injection and Parameterized Queries
* DAO Pattern

## Notes and Examples

### Review Database Design 

Review Friday's database design activity with the class

### Making Connections

- Application code that we write to interact with a database is a "client" of the database in the same way Visual Studio or PGAdmin is

- There are many different database vendors (e.g. PostgreSQL, SQL Server, Oracle, etc) that a Java or C# application may want to integrate with

    - Each platform (i.e. Java or .NET) provides an abstract interface (i.e. JDBC or ADO.NET) for interacting with a variety of databses in a generic way

    - A "driver" is implemented for each database so that application code can communicate with that database

- When we interact with a database, we need to **create a connection**.

    - Connections remain open until they are closed or time out.

    - Connections have overhead when created and opened, thus there is often a **finite number of connections**.

    <div class="definition note">

    A **connection string** specifies the name of the driver to use, the host and any port, the database name, and a username and password.
    
    </div>

    <div class="caution note">
    
    Connection strings should not be written directly in our code. **Why?**
    
    </div>

    <div class="caution note">
    Connections are valuable resources. It may not seem like a big deal if we leave it running in our single application, but what about a larger-scale application?</div>


### Executing SQL Statements

- Once a connection is instantiated and opened, it can be used by other objects that issue *SQL commands*.
- Command objects execute SQL statements and return results in the form of reader objects.
- A reader object provides the ability to walk through each row in the result set and read values from each of the columns.
- The results from a SQL query are often used to populate *domain objects*.

![xkcd Exploits of a Mom](https://imgs.xkcd.com/comics/exploits_of_a_mom.png)

[View Image](https://imgs.xkcd.com/comics/exploits_of_a_mom.png)

### SQL Injection
SQL injection occurs when untrusted data such as user data from application web pages are added to database queries, materially changing the structure and producing behaviors inconsistent with application design or purpose. 

Clever attackers exploit SQL injection vulnerabilities to steal sensitive information, bypass authentication or gain elevated privileges, add or delete rows in the database, deny services, and in extreme cases, gain direct operating system shell access, using the database as a launch point for sophisticated attacks against internal systems.

SQL injection is by far the most dangerous vulnerability impacting online applications today. When you read that hackers have stolen 450,000 user accounts or that 130 million credit card numbers have been exposed it is very likely that the application was attacked using SQL injection.

#### SQL Injection Techniques

1. **Basic Example of SQL Injection**

	- To view public messages sent by a pariticular user, the resource is:

		```
		/messages?userName=twiggy
		```

	- To load the data, the following query is sent to the database:
	
		```sql
		SELECT * FROM message WHERE private = FALSE AND sender_name = 'twiggy' ORDER BY create_date DESC
		```

	- This query is created using string concatenation:

		`... sender_name = '"+userName+"' ...`

	- What `userName` value can we use to get back ALL messages from ALL users, including private messages?

	- What happens when we send the following as the `userName` parameter?

		`' OR '1' = '1`

	- After URL encoding, the URI with the SQL Injection payload would be:

		`/messages?userName=%27%20OR%20%271%27%20%3D%20%20%271`

	- Which will result in the SQL query:

		```sql
		SELECT * FROM message WHERE private = FALSE AND sender_name = '' OR '1' = '1' ORDER BY create_date DESC
		```

2. **Using Line Comments to Ignore Parts of a Query**
	
	 - Line Comments ignore the rest of the query using database comment syntax **`--`**

	 - The previous exploit can be simplified using the following payload:

	 	`' OR 1 = 1 --`
	
	- After URL encoding, the URI with the SQL Injection payload would be:

		`/messages?userName=%27%20OR%201%20%3D%201%20--`

	- Which will result in the SQL query:

		```sql
		SELECT * FROM message WHERE private = FALSE AND sender_name = '' OR 1 = 1 --' ORDER BY create_date DESC
		```

3. **Stacking Queries - Executing more than one query in one transaction**

	- For some platforms, multiple SQL statements can be executed during one call to the database

	- This can lead to some very serious vulnerabilities.  Imagine if the `userName` parameter in the previous example contained the following payload:

		`'; DROP TABLE message; --`

	- After URL encoding, the URI with the SQL Injection payload would be:

		`/messages?userName=%27%3B%20DROP%20TABLE%20message%3B%20--`

	- Which will result in the SQL query:

		```sql
		SELECT * FROM message WHERE private = FALSE AND sender_name = ''; DROP TABLE message; --' ORDER BY create_date DESC
		```
    
4. **Union Injection - Use cross-table queries to return records from another table**

	- How might we modify the same query to retrieve all user names and passwords?

	- Consider the following payload being passed as the value of the `userName` parameter:

		`' UNION SELECT 1, '', FALSE, '', user_name || ' ' || password, NOW() FROM app_user --`

	- After URL encoding, the URI with the SQL Injection payload would be:

		`/messages?userName=%27%20UNION%20SELECT%201%2C%20%27%27%2C%20FALSE%2C%20%27%27%2C%20user_name%20%7C%7C%20%27%20%27%20%7C%7C%20password%2C%20NOW()%20FROM%20app_user%20--`

	- Which will result in the SQL query:

		```sql
		SELECT * FROM message WHERE private = FALSE AND sender_name = '' UNION SELECT 1, '', FALSE, '', user_name || ' ' || password, NOW() FROM app_user --' ORDER BY create_date DESC
		```

#### Preventing SQL Injection
1. **Parameterized Queries** - The single most effective thing you can do to prevent SQL injection is to use parameterized queries. If this is done consistently, SQL injection will not be possible. (Show this to the students in your lecture code.)

2. **Input Validation** - Limiting the data that can be input by a user can certainly be helpful in preventing SQL Injection, but is by no means an effective prevention by itself.

3. **Limit Database User Privileges** - A web application should always use a database user to connect to the database that has as few permissions as necessary.  For example, if your application never deletes data from a particular table, then the database user should not have permission to delete from that table. This will help to limit the damage in the case that there is a SQL Injection vulnerability.

### DAO Pattern

<div class="definition note">

The **Data Access Object (DAO)** design pattern encapsulates the details of persistent storage inside of classes whose only role is to store and retrieve data.

</div>

- **DAOs usually perform CRUD operations on *domain objects*.**
    - **C**reate
    - **R**ead
    - **U**pdate
    - **D**elete

- **DAO pattern makes code loosely coupled**

    - Isolating data access code inside of DAOs decouples the rest of the application from the details of persistence

        - Relational databases are often used for persistent storage, but other technologies could be used such as the filesystem, NoSQL database, web service, etc

        - Isolates the code changes that need to be made in the event of a table schema change. 

