# hola!

This project was presented as a hiring coding challenge in Mercado Libre and helped me get a position in said company.
It's also intended as an excercise of developing solutions in a DDD oriented architectural style. The solution is separated in different projects, that reflect each layer that DDD suggests as a way or organizing code and functionality.

## Technology
Dot Net Core 3.0 and Azure services.
Ocelot as the Api Gateway.

## Infrastructure
The deployed infrastructure includes a load balancing api gateway, and n-instances of the main web api. A SQL database was chosen for data persistence because of the transactional requirements of the problem.
![alt text](http://alejozito.com/github_images/diagramaInfra.JPG "Infrastructure diagram")

## REST
The API is built around 4 resources: Charges, Payments, Invoices and Events. Users could also be considered a resource, but in this solution it´s not central and was left out.
![alt text](http://alejozito.com/github_images/diagEntidades.JPG "Entitiy relationship diagram")


### Events
This starts the charges/payments flow. Executing a POST request against api/users/{userId}/events, a new charge will be created, and an event attached to it. This event stores information about the user that generated it, as well as time & date, and type.

### Charges
This entity is created as a result of creating an event. Once a charge is generated, amount, currency, user and event type are validated: if valid, an Invoice will be created.
Charges can be queried through /api/users/{userId}/chartes

### Payments
When a payment is executed through a POST, it is binded to an available charge (with an unpayed amount) and new PaymentCharges entities are created.

### Invoices
Invoices are generated when a charge is created and there were none for this user, period and currency. Once an invoice was created for this period, all new charges will be associated to this entity.
