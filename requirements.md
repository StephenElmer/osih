**Technical Assessment: Order Status Integration Hub**

**Project Overview**

Build a backend service that integrates order data from two different mock source systems and exposes a unified API for querying order status. Include a basic frontend interface to demonstrate the API functionality.

**Time Limit:** 3 hours maximum (work must stop at 3 hours regardless of completion status)

**Business Context**

You're building an integration layer for a company that receives orders through two different legacy systems. Each system uses different data formats and status codes. Your task is to create a unified way to query orders across both systems.

**Provided Materials**

You will receive two data files in a /data folder:

- system_a_orders.json - System A orders (JSON format, 12 orders)
- system_b_orders.csv - System B orders (CSV format, 13 orders)

Use these files as your data source. Do not modify the original data files.

**Requirements**

**Part 1: Data Integration (Backend)**

**System A Format (JSON):**

json

{

"orderID": "A-2024-1001",

"customer": "Acme Laboratory Services",

"orderDate": "2024-11-15",

"totalAmount": 2450.00,

"status": "PROC"

}

**System B Format (CSV):**

csv

order_num,client_name,date_placed,total,order_status

241115-5001,Acme Laboratory Services,11/15/2024,2680.00,4

**Important Notes:**

- System A uses ISO date format (YYYY-MM-DD), System B uses US format (MM/DD/YYYY)
- Your unified API should return dates in a consistent format of your choosing
- Document your choice and reasoning in the README

**Status Code Mappings:**

- System A: "PEND" = Pending, "PROC" = Processing, "SHIP" = Shipped, "COMP" = Completed, "CANC" = Cancelled
- System B: 1 = Pending, 2 = Processing, 3 = Shipped, 4 = Completed, 5 = Cancelled

**Part 2: API Development**

**Required Endpoints:**

- **GET /api/orders** - Retrieve all orders from both systems in unified format
- **GET /api/orders/{orderId}** - Retrieve a specific order by ID (from either system)
- **GET /api/health** - Health check endpoint

**Choose ONE of these optional endpoints to implement:**

- **GET /api/orders/search?status={status}** - Filter orders by normalized status (e.g., "Processing", "Completed")
- **GET /api/orders/search?startDate={date}&endDate={date}** - Filter orders by date range

**Unified Response Format:** All order data should be returned in this consistent format:

json

{

"orderId": "A-2024-1001",

"sourceSystem": "SystemA",

"customerName": "Acme Laboratory Services",

"orderDate": "2024-11-15",

"totalAmount": 2450.00,

"status": "Processing"

}

**Part 3: Basic Frontend**

Create a simple web interface that includes:

- Display of all orders OR a search interface (your choice)
- Results showing orders in a table or list format
- Clear indication of which source system each order came from
- Basic error handling for API failures

Note: Focus on functionality over design. A simple, working interface is better than an incomplete polished one.

**Part 4: Documentation**

Include a comprehensive README.md with:

- Project Overview - Brief description of your solution
- Technology Stack - What you chose and why
- Setup Instructions - Step-by-step guide to run locally
- API Documentation - Endpoints, parameters, and example responses
- Approach & Decisions - How you solved the data normalization challenge
- Time Management - What you prioritized and why
- Known Limitations - Features not completed or edge cases not handled
- Future Improvements - What you would add with more time

**Minimum Viable Solution**

To help you prioritize within the 3-hour limit, here's what constitutes a complete submission:

**Must Have**

- Can read and parse both data files
- At least 2 working API endpoints (GET all orders + GET by ID minimum)
- Data normalization (status codes and date formats unified)
- Basic frontend that displays order data
- README with setup and run instructions
- Code is organized and readable

**Nice to Have**

- Third API endpoint (search/filter functionality)
- Error handling for edge cases
- Basic tests
- Deployed to free hosting service

**What We're NOT Looking For**

- Complex database implementations (in-memory storage is perfectly acceptable)
- Production-ready security or authentication
- Extensive unit test coverage (though basic tests are welcome if time permits)
- Polished UI design (clean and functional beats pretty and incomplete)
- Handling every possible edge case
- Advanced deployment configurations

**Deliverables**

Submit a GitHub repository with this structure:

├── README.md

├── backend/ (or src/, server/, api/)

│ └── \[your backend code\]

├── frontend/ (or client/, public/)

│ └── \[your frontend code\]

├── data/

│ ├── system_a_orders.json

│ └── system_b_orders.csv

└── \[config files, package.json, requirements.txt, etc.\]

Optional: Deploy to a free hosting service (Azure, AWS, Heroku, Vercel, Render, etc.) and include the live URL in your README.

**Evaluation Criteria**

Your submission will be evaluated in this priority order:

- **Documentation Clarity (Highest Priority)** - Can someone understand your approach and run your project easily?
- **Problem-Solving Approach** - How did you handle data normalization and integration challenges?
- **Code Quality & Organization** - Is your code readable, maintainable, and well-structured?
- **Prioritization** - How did you manage the 3-hour constraint? What did you choose to build vs. defer?
- **Error Handling** - How does your system handle edge cases and failures?
- **Feature Completeness** - What percentage of requirements did you implement?
