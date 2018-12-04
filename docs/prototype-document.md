[docdb]: https://en.wikipedia.org/wiki/Document-oriented_database

# Overview

"The central concept of a (document-oriented database)[dpcdb] is the notion of a document. 
While each document-oriented database implementation differs on the details of this definition, in general, they all assume documents
encapsulate and encode data (or information) in some standard format or encoding."

Our design approach is model-first approach meaning we start with a prototype document that represents one Scrapbook 101 item. The values of the fields don't matter right now, only the structure.

```json
{
    "id": "8bb3e432-0705-4cbc-99d4-b22853a8bc4b",
    "type": "scrapbook101Item",
    "assetPath": "path to assets",
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
# Description of fields

<dl>
    <dt>id</dt>
    <dd>This will be an auto-generated GUID.
    </dd>
    <dt>type</dt>
    <dd>Describes the type of record. If you records are stored in a Document DB store with other records with auto-generated
        IDs then type helps distinguish Scrapbook 101 records uniquely. Also, as you expand the functionality of Scrapbook 101,
        other type of records may be needed like "scrapbook101CategoryList".  <strong>Default</strong>: scrapbook101Item
    </dd>
    <dt>assetPath</dt>
    <dd>
    </dd>
    <dt>dataAdded</dt>
    <dd>
    </dd>
    <dt>dateUpdated</dt>
    <dd>
    </dd>
    <dt>updatedBy</dt>
    <dd>
    </dd>
</dl>
