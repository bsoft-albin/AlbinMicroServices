
// need to write my ReadMe File contenst Here.


================= Directory Puposes =====================

Application (Business Logics Layer)   // Application logic, Use cases, CQRS
Domain   (Business Rules)     // Domain entities,Business rules , aggregates, value objects
Infrastructure (Repsitory and Context Layer) // Data access, third-party integrations


===> in the above i'm going to change one thing, according to me
* => Write Domain Rules(Model class prop validation), define entities and DTOs here, also Business Logics (Use Cases) and CQRS here not in application Layer.
* => So, in Application Layer Purely Used for (Model validation using Attributes or (Manual Model Verfication method for Each Model), Response Object cooking, Status codes (custom codes also), also which Business Logic Method need to call , deciding here)