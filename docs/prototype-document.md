[1]: https://en.wikipedia.org/wiki/Document-oriented_database

"The central concept of a (document-oriented database)[1] is the notion of a document. 
While each document-oriented database implementation differs on the details of this definition, in general, they all assume documents
encapsulate and encode data (or information) in some standard format or encoding."

Design here is model-first approach meaning we start with a prototype document that represents one Scrapbook item.

```
{
    "id": "8bb3e432-0705-4cbc-99d4-b22853a8bc4b",
    "type": "scrapbook101Item",
    "folderPath": "YYYY-MM-DD",
    "dateAdded": "timestamp",
    "dateUpdated": "timestamp",
    "updatedBy": "email adress",
    "location": "location name",
    "geoLocation": {
        "type": "Point",
        "coordinates": [
            9.66950988769531,
            45.6952285766602
        ]
    },
    "category": "Books",
    "rating": "score",
    "title": "book title",
    "description": "description",
    "categoryFields": {
        "author": "author name",
        "date": "date",
        "isbn": "isbn",
        "synopsis": "synopsis",
        "theme": "theme",
        "title": "title of book"
    },
    "assets": [
        {
            "name": "CoverFront.jpg",
            "size": "17904",
            "role": "main"
        },
        {
            "name": "CoverBack.jpg",
            "size": "17904",
            "role": "asset"
        }
    ],
}

```
