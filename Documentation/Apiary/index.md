"value": "1212.xml"FORMAT: 1A


# OnDemand Tools API

## Important Note
If you retrieve XML data from our API, please make sure to set HTTP protocol version to 1.0. Versions 1.1 and later
introduces chunking which can result in weird characters appearing within the XML payload.

## Getting Started

### Environments

ODT API environments:

DEV : https://dev.api.odt.turner.codes/

QA  : https://qa.api.odt.turner.codes/

PROD: https://api.odt.turner.codes/

Note that the examples provided here points to a mock host (http://private-71d74-ondemandtools.apiary-mock.com). Please replace it with one of the above mentioned ODT API host when testing from your application.
Access to ODT API route is restricted through registered API key. If you need to setup a new key, please contact the ODT team- OnDemandToolsSupport@turner.com

### Message Queue Server

Our message queue servers are located at

DEV : amqp://ondemand-tools-queues-dev.turner.com

QA  : amqp://ondemand-tools-queues-dev.turner.com

PROD: amqp://queue.odt.turner.com

However, the user, password, and vhost are distributed only to users of our system, so if you need access to our servers please reach out to the ODT team to get proper access.

# Group Airing Resource

Airing resource related routes and operations

## Retrieve airing by ID [/v1/airing/{airingId}?options={opValue}]

### GET [GET]

Retrieves non deleted airing with the given ID. The retrieval process is permission based, therefore, verify that the provided API key has permission to the correct destination. Additionally, the airing should have at least one flight window.

#### How to infer parameter - 'options'
Parameter 'options' is useful if you want to augment additional information to the airing.

     Here are the available values 
            
            file: all files (video, non-video) registered with this airing
            title: all titles for this airing
            destination: destinations for this airing. Restriction based on API key privileges
            status: statuses pertinent to this airing. 
                    If video=true, it indicates that this airing has all the required video information 
                    If MEDIUM=true, it indicates that medium service has completed their part of the workflow
            package: all packages that match this airing by TitleIds, ContentIds, AiringId and Destination
            series: all series and corresponding files related to the series.
            premiere: premiere data as reflected in Flow/Title - http://docs.turner.com/display/DataCity/V2+-+First+Title+Airings


You can include one or more values using the pipe
operation.

     Here are some examples 
            
            file only:                      options=file
            file and title:                 options=file|title
            for file, title & destination:  options=file|title|destination
            for file & status:              options=file|status
            file and series:                options=series
            for package:                    options=package


+ Parameters

    + airingId: TBSD1009241300097990 (string, required) - airing ID
    + opValue:  file|title|series|destination|status|premiere (string, optional) -  retrieve additional information (file, title, destination, status) pertinent to the airing. You may choose to include one or all pertinent information. 
                
             

+ Request Airing

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json 
            Accept: application/json       
    
+ Response 200 (application/json)

    + Body
    
            {
              "airingId": "TBSD1009241300097990",
              "mediaId": "b3cb5a53a50741b3ba6d333ca20f6d3ad1ee3eb6",
              "name": "Episode 1007",
              "type": "Episode (Non-Animated)",
              "brand": "TBS",
              "platform": "Cable",
              "airings": [
                {
                  "date": "2013-11-07T05:00:00Z",
                  "airingId": 7066880,
                  "linked": true,
                  "authority":"Turner"
                }
              ],
              "duration": {
                "lengthInSeconds": 950399,
                "displayMinutes": 15840
              },
              "title": {
                "rating": {
                  "code": "TV-MA",
                  "description": "L"
                },
                "storyLine": {
                  "long": "TBS Presents: \"Pete Holmes\" Guests: Casey Wilson & June Diane Raphael",
                  "short": ""
                },
                "releaseYear": 2013,
                "keywords": "Pete Holmes, TBS, Series",
                "originalPremiereDate": "2013-11-06T05:00:00Z",
                "episode": {
                  "name": "Episode 1007",
                  "number": "1007"
                },
                "series": {
                  "name": "Pete Holmes",
                  "id": 2004630
                },
                "season": {
                  "name": "Season 1",
                  "number": 1
                },
                "participants": [],
                "titleIds": [
                  {
                    "type": "Episode (Non-Animated)",
                    "value": "2004637",
                    "authority": "Turner",
                    "primary": true
                  }
                ],
                "relatedTitleIds": [
                  {
                    "type": "Series",
                    "value": "2004630",
                    "authority": "Turner",
                    "primary": false
                  }
                ]
              },
              "flights": [
                {
                  "start": "2013-11-10T08:30:00Z",
                  "end": "2013-11-21T08:29:59Z",
                  "destinations": [
                    {
                      "externalId": 0,
                      "name": "DTV",
                      "authenticationRequired": false,
                      "package": {
                        "packageName": "Pete Holmes 1007",
                        "fileName": "Pete Holmes 1007",
                        "titleDigital": "Episode 1007",
                        "titleBrief": "Pete Holmes 1007",
                        "genres": [
                          "Series"
                        ],
                        "subGenres": [],
                        "contentTiers": [],
                        "productCodes": [
                          "TBSOD"
                        ],
                        "guideCategories": [
                          "TV Series;Comedy"
                        ],
                        "programTypes": [
                          "series"
                        ],
                        "categories": []
                      },
                      "properties": [
                        {
                          "name": "Description",
                          "value": "Dish for Broadband",
                          "brands": [],
                          "titleIds": [],
                          "seriesIds": []
                        },
                        {
                          "name": "IP Address",
                          "value": "12.109.233.27",
                          "brands": ["TBS"],
                          "titleIds": [],
                           "seriesIds": []
                        }],
                      "deliverables": [ 
                      {
                           "value": "TBSD1009241300097990.xml"
                      } ]
                    },
                    {
                      "externalId": 0,
                      "name": "TVN",
                      "authenticationRequired": false,
                      "package": {
                        "packageName": "Pete Holmes 1007",
                        "fileName": "Pete Holmes 1007",
                        "titleDigital": "Episode 1007",
                        "titleBrief": "Pete Holmes 1007",
                        "genres": [
                          "Comedy"
                        ],
                        "subGenres": [],
                        "contentTiers": [
                          "TBS_10"
                        ],
                        "productCodes": [
                          "TBSOD"
                        ],
                        "guideCategories": [],
                        "programTypes": [],
                        "categories": []
                        
                      },
                      "properties": [
                      {
                        "name": "Description",
                        "value": "Dish for Broadband",
                        "brands": [],
                        "titleIds": [],
                        "seriesIds": []
                      },
                      {
                        "name": "IP Address",
                        "value": "12.109.233.27",
                        "brands": ["TBS"],
                        "titleIds": [],
                        "seriesIds": []
                      }],
                    "deliverables": [ 
                    {
                        "value": "TBSD1009241300097990.xml"
                    }]
                    },
                  "tags": []
                }
              ],
              "flags": {
                "hd": false,
                "cx": false,
                "programmerBrandingReq": false,
                "fastForwardAllowed": false,
                "manuallyProcess": false,
                "stacked": false
              },
              "versions": [
                {
                  "contentId": "20X11",
                  "closedCaptioning": {
                    "file": false,
                    "encode": true
                  }
                }
              ],
              "playList": [
                {
                  "position": 1,
                  "id": "100FG",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 1,
                  "id": "202GW",
                  "type": "CID",
                  "itemType": "Commercial",
                  "idType": "CID"
                },
                {
                  "position": 1,
                  "id": "100FG",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 1,
                  "id": "20X11/01",
                  "type": "MaterialID",
                  "itemType": "Segment",
                  "idType": "MaterialID"
                },
                {
                  "position": 1,
                  "id": "202GW",
                  "type": "CID",
                  "itemType": "Commercial",
                  "idType": "CID"
                },
                {
                  "position": 2,
                  "id": "20RY8",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 2,
                  "id": "20X11/02",
                  "type": "MaterialID",
                  "itemType": "Segment",
                  "idType": "MaterialID"
                },
                {
                  "position": 1,
                  "id": "1KYDM",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 2,
                  "id": "208Y6",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 3,
                  "id": "20X11/03",
                  "type": "MaterialID",
                  "itemType": "Segment",
                  "idType": "MaterialID"
                },
                {
                  "position": 1,
                  "id": "20RYH",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 4,
                  "id": "20X11/04",
                  "type": "MaterialID",
                  "itemType": "Segment",
                  "idType": "MaterialID"
                },
                {
                  "position": 2,
                  "id": "100FH",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                },
                {
                  "position": 1,
                  "id": "202GW",
                  "type": "CID",
                  "itemType": "Commercial",
                  "idType": "CID"
                },
                {
                  "position": 2,
                  "id": "100FH",
                  "type": "CID",
                  "itemType": "Promo",
                  "idType": "CID"
                }
              ],
              "deviceExclusions": [],
              "webFlags": [],
              "releasedOn": "2013-11-05T16:10:43.313Z",
              "releasedBy": "clott",
              "options": {
                "files": [],
                "titles": [],
                "series": [],
                "changes": [],
                "destinations": [],
                "premieres":[],
                "packages": [],
                "status":{
                    "video":true,
                    "MEDIUM":true,
                }
              },
              "properties": {}
            }

+ Response 404 (application/json)

    + Body
    
            {
              "message": "Airing does not exist in current collection.",
              "exception": "..."
            }
                      
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have correct privileges-->
             []
    
            <!-- Possible cause:
                * your API key doesn't have permisison to the destination(s) where this airing is destined
                * airing doesn't have a destination/flight
            -->
            {
              "message": "Request denied for TBSD1009241300097990 airing.",
              "exception": "..."
            }
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
             

                   
            
## Retrieve airings by title ID [/v1/airings/titleId/{titleId}]

### GET[GET]

    Retrieve non expired airings (active flight window) that match the given title ID. This operation is permission based, therefore, verify that the API key has permission to correct destination and brand.

+ Parameters

    + titleId: 642918 (required, number) - title ID

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json

+ Response 200 (application/json)

    + Body
            
            <!-- Empty reponse. It could mean:
                * either there are no airings that match the given title ID, or
                * the matching airings belong to a destination to which your API key doesn't have permisison, or
                * the matching airings doesn't have a destination, or
                * the matching airings have expired
            -->
            [] 
    
            <!-- Returned content for matched airings -->
            [
              {
                "airingId": "CART1007071600024964",
                "mediaId": "c79d510d02de89fb2cb2520bcf525f814657205c"
                "name": "300",
                "type": "Feature Film",
                "brand": "Cartoon",
                "platform": "Cable",
                "duration": {
                  "lengthInSeconds": 6217,
                  "displayMinutes": 105
                },
                "title": {
                  "rating": {
                    "code": "TV-14",
                    "description": "V,S"
                  },
                  "storyLine": {
                    "long": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and y.",
                    "short": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army."
                  },
                  "releaseYear": 2007,
                  "keywords": "Allegiance, Armies, Campaigns & battles, Cities & towns, Courage, Daggers & swords, Death, Dictators, Emperors, Ethnic groups, Greece, Honor, Kings, Liberty, Military tactics, Politicians, Pride, Queens, Religion, Shields, Soldiers, Spears, Traitors, Violence, War",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "642918",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV001818050000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-07-08T04:00:00Z",
                    "end": "2016-08-12T03:59:59Z",
                    "destinations": [
                      {
                        "externalId": 1,
                        "name": "TVN",
                        "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["TBS"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "CART1007071600024964.xml"
                          }
                      }
                    ],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false,
                  "stacked": false
                },
                "releasedOn": "2016-07-07T14:29:07.43Z",
                "releasedBy": "c-xpanxion-ganesan"
              },
              {
                "airingId": "CART1006071600022675",
                "name": "300 updated 10.",
                "type": "Feature Film",
                "brand": "Cartoon",
                "platform": "Cable",
                "duration": {
                  "lengthInSeconds": 6217,
                  "displayMinutes": 105
                },
                "title": {
                  "rating": {
                    "code": "TV-14",
                    "description": "V,S"
                  },
                  "storyLine": {
                    "long": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army. ",
                    "short": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army."
                  },
                  "releaseYear": 2007,
                  "keywords": "Allegiance, Armies, Campaigns & battles, Cities & towns, Courage, Daggers & swords, Death, Dictators, Emperors, Ethnic groups, Greece, Honor, Kings, Liberty, Military tactics, Politicians, Pride, Queens, Religion, Shields, Soldiers, Spears, Traitors, Violence, War",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "642918",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV001818050000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-06-08T04:00:00Z",
                    "end": "2016-07-13T03:59:59Z",
                    "destinations": [
                      {
                        "externalId": 1,
                        "name": "TVN",
                        "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["Cartoon"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "1212.xml"
                         } ],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false
                },
                "releasedOn": "2016-06-13T14:00:31.434Z",
                "releasedBy": "SyncTitleDataTask"
              },
              {
                "airingId": "TRUE1006141600008081",
                "name": "300 updated 101!",
                "type": "Feature Film",
                "brand": "TRUTV",
                "platform": "Broadband",
                "duration": {
                  "lengthInSeconds": 6217,
                  "displayMinutes": 105
                },
                "title": {
                  "rating": {
                    "code": "TV-14",
                    "description": "S,V"
                  },
                  "storyLine": {
                    "long": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army. Facing insurmountable odds, their valor and sacrifice inspire all of Greece to unite against their Persian enemy, drawing a line in the sand for democracy.",
                    "short": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army."
                  },
                  "releaseYear": 2007,
                  "keywords": "Allegiance, Armies, Campaigns & battles, Cities & towns, Courage, Daggers & swords, Death, Dictators, Emperors, Ethnic groups, Greece, Honor, Kings, Liberty, Military tactics, Politicians, Pride, Queens, Religion, Shields, Soldiers, Spears, Traitors, Violence, War",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "642918",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV001818050000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2017-06-08T07:00:00Z",
                    "end": "2017-06-16T06:59:59Z",
                    "destinations": [
                      {
                        "externalId": 2,
                        "name": "CIM",
                         "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["Cartoon"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": []
                      }
                    ],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false
                },
                "releasedOn": "2016-06-17T05:06:35.356Z",
                "releasedBy": "SyncTitleDataTask"
              },
              {
                "airingId": "TBSE1006201600032794",
                "name": "300",
                "type": "Feature Film",
                "brand": "TBS",
                "platform": "Broadband",
                "duration": {
                  "lengthInSeconds": 6217,
                  "displayMinutes": 105
                },
                "title": {
                  "rating": {
                    "code": "TV-14",
                    "description": "S,V"
                  },
                  "storyLine": {
                    "long": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army. Facing insurmountable odds, their valor and sacrifice inspire all of Greece to unite against their Persian enemy, drawing a line in the sand for democracy.",
                    "short": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army."
                  },
                  "releaseYear": 2007,
                  "keywords": "Allegiance, Armies, Campaigns & battles, Cities & towns, Courage, Daggers & swords, Death, Dictators, Emperors, Ethnic groups, Greece, Honor, Kings, Liberty, Military tactics, Politicians, Pride, Queens, Religion, Shields, Soldiers, Spears, Traitors, Violence, War",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "642918",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV001818050000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-06-21T07:00:00Z",
                    "end": "2016-07-12T06:59:59Z",
                    "destinations": [
                      {
                        "externalId": 2,
                        "name": "CIM",
                        "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "1212.xml"
                         }],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false
                },
                "releasedOn": "2016-06-30T08:01:15.447Z",
                "releasedBy": "SyncTitleDataTask"
              },
              {
                "airingId": "TBSE1006201600032795",
                "name": "300",
                "type": "Feature Film",
                "brand": "TBS",
                "platform": "Broadband",
                "duration": {
                  "lengthInSeconds": 6217,
                  "displayMinutes": 105
                },
                "title": {
                  "rating": {
                    "code": "TV-14",
                    "description": "S,V"
                  },
                  "storyLine": {
                    "long": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army. Facing insurmountable odds, their valor and sacrifice inspire all of Greece to unite against their Persian enemy, drawing a line in the sand for democracy.",
                    "short": "In the ancient Battle of Thermopylae, King Leonidas and 300 Spartans fought to the death against Xerxes and his massive Persian army."
                  },
                  "releaseYear": 2007,
                  "keywords": "Allegiance, Armies, Campaigns & battles, Cities & towns, Courage, Daggers & swords, Death, Dictators, Emperors, Ethnic groups, Greece, Honor, Kings, Liberty, Military tactics, Politicians, Pride, Queens, Religion, Shields, Soldiers, Spears, Traitors, Violence, War",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "642918",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV001818050000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-06-21T07:00:00Z",
                    "end": "2016-07-12T06:59:59Z",
                    "destinations": [
                      {
                        "externalId": 2,
                        "name": "CIM",
                        "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["TBS"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "1212.xml"
                          }],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false
                },
                "releasedOn": "2016-06-30T08:01:22.312Z",
                "releasedBy": "SyncTitleDataTask"
              }  
            ]

            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
             
            
            
            
## Retrieve airings by series ID [/v1/airings/seriesId/{seriesId}]

### GET[GET]

    Retrieve non expired airings (active flight window) that match the given title series ID. This operation is permission based, therefore, verify that the API key has permission to the correct destination and brand.

+ Parameters

    + seriesId: 980450 (required, number) - Series ID

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json

+ Response 200 (application/json)

    + Body
            
            <!-- Empty reponse. It could mean:
                * either there are no airings that match the given title series ID, or
                * the matching airings belong to a destination to which your
                   API key doesn't have permisison, or
                * the matching airings doesn't have a destination, or
                * the matching airings have expired
            -->
            [] 
    
            <!-- Returned content for matched airings -->
            [
              {
                "airingId": "CNNX1012041300062589",
                "mediaId": "041a1a0480759a5267c42fe4ae14c95a9177dc95",
                "name": "Dr Drew 2013-Dec-05",
                "type": "News Episode",
                "brand": "Headline",
                "platform": "Cable",
                "duration": {
                  "lengthInSeconds": 604799,
                  "displayMinutes": 10080
                },
                "title": {
                  "rating": {
                    "code": "NR",
                    "description": ""
                  },
                  "storyLine": {
                    "long": "Dubbed 'the pillowcase rapist', a man who admitted to raping 38 women in the 80's and 90's may be released in just a few weeks. The residents of the neighborhood where he's been cleared to live are outraged.\r",
                    "short": "As a physician specializing in addiction medicine, Dr."
                  },
                  "releaseYear": 2013,
                  "keywords": "Dr. Drew on Call, HDLN, News",
                  "episode": {
                    "name": "Dr Drew 2013-Dec-05",
                    "number": "2013-12-05"
                  },
                  "series": {
                    "name": "Dr. Drew on Call",
                    "id": 980450
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "News Episode",
                      "value": "2012338",
                      "authority": "Turner"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2013-12-06T06:00:00Z",
                    "end": "2016-12-13T05:59:59Z",
                    "destinations": [
                      {
                        "externalId": 0,
                        "name": "TVN",
                        "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["Headline"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "1212.xml"
                          }],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": true,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false,
                  "stacked": true
                },
                "releasedOn": "2013-12-06T04:14:53.49Z",
                "releasedBy": "iachinoc"
              }
            ]
            

+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
             
             
## Retrieve airings by media ID [/v1/airings/mediaid/{mediaId}?startDate={startDate}&endDate={endDate}]

### GET[GET]

    Retrieve non expired airings (active flight window) that match the given media ID. This operation is permission based, therefore, verify that the API key has permission to the correct destination and brand.
    Active flight window is a date range to get number of airings that are running in the given interval.
    StartDate provided in the input can be any date that's not greater than actual Flight End date and EndDate in the input can be any date that's not less than actual Flight Start date.

+ Parameters

    + mediaId:   041a1a0480759a5267c42fe4ae14c95a9177dc95 (required, string) - Media Id
    + startDate:   06/10/2016 (optional, string) - Start date can be any date that's not greater than Flight end date.
    + endDate:     06/11/2016 (optional, string) - End date can be any date that's not less than Flight start date.
                

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json

+ Response 200 (application/json)

    + Body
           
            <!-- Empty reponse. It could mean:
                * either the matching airings belong to a destination to 
                   which your API key doesn't have permisison, or
                * the matching airings doesn't have a destination.
            -->
            
            <!-- Returned content for matched airings -->
            [
              {
                "airingId": "CNNX1012041300062589",
                "name": "Dr Drew 2013-Dec-05",
                "type": "News Episode",
                "brand": "Headline",
                "platform": "Cable",
                "mediaId": "041a1a0480759a5267c42fe4ae14c95a9177dc95",
                "duration": {
                  "lengthInSeconds": 604799,
                  "displayMinutes": 10080
                },
                "title": {
                  "rating": {
                    "code": "NR",
                    "description": ""
                  },
                  "storyLine": {
                    "long": "Dubbed 'the pillowcase rapist', a man who admitted to raping 38 women in the 80's and 90's may be released in just a few weeks. The residents of the neighborhood where he's been cleared to live are outraged.\r",
                    "short": "As a physician specializing in addiction medicine, Dr."
                  },
                  "releaseYear": 2013,
                  "keywords": "Dr. Drew on Call, HDLN, News",
                  "episode": {
                    "name": "Dr Drew 2013-Dec-05",
                    "number": "2013-12-05"
                  },
                  "series": {
                    "name": "Dr. Drew on Call",
                    "id": 980450
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "News Episode",
                      "value": "2012338",
                      "authority": "Turner"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2013-12-06T06:00:00Z",
                    "end": "2016-12-13T05:59:59Z",
                    "destinations": [
                      {
                        "externalId": 0,
                        "name": "TVN",
                        "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["Headline"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "1212.xml"
                          }],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": true,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false,
                  "stacked": false
                },
                "releasedOn": "2013-12-06T04:14:53.49Z",
                "releasedBy": "iachinoc"
              }
            ]
            

+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []   
          
            
+ Response 404 (application/json)

    + Body
    
              {
              "message": "No Airings found for media id 041a1a0480759a5267c42fe4ae14c95a9177dc95."
              }
            or
             {
              "message": "No Airings found for media id 041a1a0480759a5267c42fe4ae14c95a9177dc95 with selected date range."
             }
             
+ Response 412 (application/json)

    + Body
    
              {
              "message": "Start date should be smaller than the end date."
              }

+ Response 204 (application/json)

    
            
            
## Retrieve airings by destination [/v1/airings/destination/{destination}]

### GET[GET]

    Retrieve non expired airings (active flight window) that match the given destination name. This operation is permission based. Verify that the API key has permission to the correct destination and brands. 
    
    Examples of some destination names:
         TLTN - TeleToons
         CNNR - CNN Go Roku platform
         CIM  - CIM/Xfinity

+ Parameters

    + destination: CIM (required, string) - destination name

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json

+ Response 200 (application/json)

    + Body
            
            <!-- Empty reponse. It could mean:
                * either there are no airings that match the given destination name, or
                * the matching airings belong to a destination/brand to which your API key doesn't have permisison, or
                * the provided destination is not valid
            -->
            [] 
            
            <!-- Returned content for matched airings -->
            [
                 {
                    "airingId": "TBSE1006011600032168",
                    "mediaId": "65bdf04384a027246a4d4faf9a2223b3612368f1",
                    "name": "Like a Boss",
                    "type": "Episode (Animated)",
                    "brand": "TBS",
                    "platform": "Broadband",
                    "duration": {
                      "lengthInSeconds": 1229,
                      "displayMinutes": 30
                    },
                    "title": {
                      "rating": {
                        "code": "TV-14",
                        "description": "D,L,S,V"
                      },
                      "storyLine": {
                        "long": "Cleveland attempts to sabotage Tim after he gets a promotion at Waterman Cable; Rallo tricks Junior into thinking his pet turtle talks.test",
                        "short": "Cleveland attempts to sabotage Tim after he gets a promotion at Waterman Cable; Rallo tricks Junior into thinking his pet turtle talks."
                      },
                      "releaseYear": 2011,
                      "keywords": "Cleveland Show, The, TBS, Comedy",
                      "episode": {
                        "name": "Like a Boss",
                        "number": "211"
                      },
                      "series": {
                        "name": "Cleveland Show, The",
                        "id": 809331
                      },
                      "season": {
                        "name": "Season 2",
                        "number": 2
                      },
                      "titleIds": [
                        {
                          "type": "Episode",
                          "value": "809371",
                          "authority": "Turner"
                        },
                        {
                          "type": "Episode",
                          "value": "EP010591100036",
                          "authority": "TMS"
                        }
                      ]
                    },
                    "flights": [
                      {
                        "start": "2016-07-06T18:30:00Z",
                        "end": "2016-07-13T18:29:59Z",
                        "destinations": [
                          {
                            "externalId": 2,
                            "name": "CIM",
                            "properties": [
                         {
                            "name": "Description",
                            "value": "Dish for Broadband",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["Cartoon"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [ 
                         {
                           "value": "1212.xml"
                          }]
                          },
                          {
                            "externalId": 1,
                            "name": "TVN",
                            "properties": [
                         {
                            "name": "username",
                            "value": "comcast",
                            "brands": [],
                            "titleIds": [],
                            "seriesIds": []
                         },
                         {
                            "name": "IP Address",
                            "value": "12.109.233.27",
                            "brands": ["Cartoon"],
                            "titleIds": [],
                            "seriesIds": []
                         }],
                        "deliverables": [] 
                          },
                          {
                            "externalId": 0,
                            "name": "CMA",
                            "properties": [],
                        "deliverables": [ 
                         {
                           "value": "12912.xml"
                          }]
                          },
                          {
                            "externalId": 3,
                            "name": "DISHB",
                            "properties": [],
                            "deliverables": [] 
                         
                          },
                          {
                            "externalId": 21,
                            "name": "TBSW",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 23,
                            "name": "VZN",
                             "properties": [],
                            "deliverables": [] 
                          }
                        ],
                        "tags": []
                      }
                    ],
                    "flags": {
                      "hd": false,
                      "cx": false,
                      "programmerBrandingReq": false,
                      "fastForwardAllowed": false,
                      "manuallyProcess": false,
                      "stacked": false
                    },
                    "releasedOn": "2016-06-30T10:28:42.61Z",
                    "releasedBy": "c-xpanxion-ranandan"
                  },
                  {
                    "airingId": "TBSE1006011600032170",
                    "name": "Short Story and a Tall Tale, A",
                    "type": "Episode (Animated)",
                    "brand": "TBS",
                    "platform": "Broadband",
                    "duration": {
                      "lengthInSeconds": 1228,
                      "displayMinutes": 30
                    },
                    "title": {
                      "rating": {
                        "code": "TV-14",
                        "description": "D,L,S,V"
                      },
                      "storyLine": {
                        "long": "Rallo incurs a diminutive mobster's wrath after reneging on a promise to marry the mobster's sister.test",
                        "short": "Rallo incurs a diminutive mobster's wrath after reneging on a promise to marry the mobster's sister."
                      },
                      "releaseYear": 2011,
                      "keywords": "Cleveland Show, The, TBS, Comedy",
                      "episode": {
                        "name": "Short Story and a Tall Tale, A",
                        "number": "214"
                      },
                      "series": {
                        "name": "Cleveland Show, The",
                        "id": 809331
                      },
                      "season": {
                        "name": "Season 2",
                        "number": 2
                      },
                      "titleIds": [
                        {
                          "type": "Episode",
                          "value": "809372",
                          "authority": "Turner"
                        },
                        {
                          "type": "Episode",
                          "value": "EP010591100037",
                          "authority": "TMS"
                        }
                      ]
                    },
                    "flights": [
                      {
                        "start": "2016-07-06T19:00:00Z",
                        "end": "2016-07-13T18:59:59Z",
                        "destinations": [
                          {
                            "externalId": 2,
                            "name": "CIM",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 1,
                            "name": "TVN",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 0,
                            "name": "CMA",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 3,
                            "name": "DISHB",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 21,
                            "name": "TBSW",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 23,
                            "name": "VZN",
                            "properties": [],
                            "deliverables": [] 
                          }
                        ],
                        "tags": []
                      }
                    ],
                    "flags": {
                      "hd": false,
                      "cx": false,
                      "programmerBrandingReq": false,
                      "fastForwardAllowed": false,
                      "manuallyProcess": false,
                      "stacked": false
                    },
                    "releasedOn": "2016-06-30T10:28:56.517Z",
                    "releasedBy": "c-xpanxion-ranandan"
                  },
                  {
                    "airingId": "CARE1003291600010394",
                    "name": "Strong-Armed",
                    "type": "Episode (Animated)",
                    "brand": "Cartoon",
                    "platform": "Broadband",
                    "duration": {
                      "lengthInSeconds": 676,
                      "displayMinutes": 15
                    },
                    "title": {
                      "rating": {
                        "code": "TV-Y7-FV",
                        "description": ""
                      },
                      "storyLine": {
                        "long": "When Bubbles breaks her arm and gets a powerful robotic cast, she must learn to stop being timid and embrace her smashy side to defeat the sleazy, sinister Pack Rat!",
                        "short": "When Bubbles breaks her arm and gets a powerful robotic cast, she must learn to stop being timid and embrace her smashy side to defeat the sleazy, sinister Pack Rat!"
                      },
                      "releaseYear": 2016,
                      "keywords": "Powerpuff Girls, The, CTN, Action, Animated, Comedy",
                      "episode": {
                        "name": "Strong Armed",
                        "number": "105"
                      },
                      "series": {
                        "name": "Powerpuff Girls, The",
                        "id": 2064567
                      },
                      "season": {
                        "name": "Season 1",
                        "number": 1
                      },
                      "titleIds": [
                        {
                          "type": "Episode",
                          "value": "2083904",
                          "authority": "Turner"
                        },
                        {
                          "type": "Episode",
                          "value": "EP023789040013",
                          "authority": "TMS"
                        }
                      ]
                    },
                    "flights": [
                      {
                        "start": "2016-04-14T01:15:00Z",
                        "end": "2016-07-14T01:14:59Z",
                        "destinations": [
                          {
                            "externalId": 0,
                            "name": "CMA",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 2,
                            "name": "CIM",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 3,
                            "name": "DISHB",
                             "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 7,
                            "name": "ATTB",
                            "properties": [],
                            "deliverables": [] 
                          },
                          {
                            "externalId": 23,
                            "name": "VZN",
                            "properties": [],
                            "deliverables": [] 
                          }
                        ],
                        "tags": []
                      }
                    ],
                    "flags": {
                      "hd": false,
                      "cx": false,
                      "programmerBrandingReq": false,
                      "fastForwardAllowed": false,
                      "manuallyProcess": false,
                      "stacked": false
                    },
                    "releasedOn": "2016-06-24T19:18:30.4Z",
                    "releasedBy": "c-xpanxion-ganesan"
                  }
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
## Retrieve airings by brand, destination and flight window [/v1/airings/brand/{brand}/destination/{destination}?startDate={startDate}&endDate={endDate}]

### GET[GET]

    Retrieve airings that match the given destination name, brand and flight window. This operation is permission based. Verify that the API key has permission to the correct destination and brands. 
    
    Examples of some destination names:
         TLTN - TeleToons
         CNNR - CNN Go Roku platform
         CIM  - CIM/Xfinity

    Examples of some brands:
         TCM
         TNT
         TRUTV
         
    Duration of the flight window will determine the response time for this route
    
+ Parameters

    + brand:       TNT (required, string) - airing brand
    + destination: CIM (required, string) - destination name
    + startDate:   06/10/2016 (required, string) - airing flight start date less than or equal to this date
    + endDate:     06/11/2016 (required, string) - airing flight end date greater than or equal to this date

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json

+ Response 200 (application/json)

    + Body
            
            <!-- Empty reponse. It could mean:
                * either there are no airings that match the given criteria, or
                * the matching airings belong to a destination/brand to which your API key doesn't have permisison, or
                * the provided destination or brand is not valid
            -->
            [] 
            
            <!-- Returned content for matched airings -->
            [
              {
                "airingId": "TCME1006081600011578",
                "mediaId: "c00cbc4f081b334ae682b22c4d15ae3df56be58e",
                "name": "Cyrano De Bergerac",
                "type": "Feature Film",
                "brand": "TCM",
                "platform": "Broadband",
                "duration": {
                  "lengthInSeconds": 6720,
                  "displayMinutes": 120
                },
                "title": {
                  "rating": {
                    "code": "TV-PG",
                    "description": ""
                  },
                  "storyLine": {
                    "long": "A swordsman and poet helps another man woo the woman he loves.",
                    "short": "A swordsman and poet helps another man woo the woman he loves."
                  },
                  "releaseYear": 1950,
                  "keywords": "Cyrano De Bergerac, Mala Powers, William Prince, Jose Ferrer, TCM",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "72069",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV000075790000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-06-09T15:30:00Z",
                    "end": "2016-07-14T15:29:59Z",
                    "destinations": [
                      {
                        "externalId": 2,
                        "name": "CIM",
                        "properties": [],
                        "deliverables": [] 
                      }
                    ],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false,
                  "stacked": false
                },
                "releasedOn": "2016-06-08T10:13:46.713Z",
                "releasedBy": "Linear Promo Refresh"
              },
              {
                "airingId": "TCME1006081600011579",
                "name": "Green Pastures, The",
                "type": "Feature Film",
                "brand": "TCM",
                "platform": "Broadband",
                "duration": {
                  "lengthInSeconds": 5580,
                  "displayMinutes": 105
                },
                "title": {
                  "rating": {
                    "code": "TV-G",
                    "description": ""
                  },
                  "storyLine": {
                    "long": "God tests the human race in this reenactment of Bible stories set in the world of black American folklore.",
                    "short": "God tests the human race in this reenactment of Bible stories set in the world of black American folklore."
                  },
                  "releaseYear": 1936,
                  "keywords": "Green Pastures, The, Oscar Polk, Eddie Anderson, Rex Ingram, TCM",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "1890",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV000084220000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-06-09T17:15:00Z",
                    "end": "2016-07-14T17:14:59Z",
                    "destinations": [
                      {
                        "externalId": 2,
                        "name": "CIM",
                        "properties": [],
                        "deliverables": [] 
                      }
                    ],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false
                },
                "releasedOn": "2016-06-08T10:13:51.573Z",
                "releasedBy": "Linear Promo Refresh"
              },
              {
                "airingId": "TCME1006081600011580",
                "name": "Annie Get Your Gun",
                "type": "Feature Film",
                "brand": "TCM",
                "platform": "Broadband",
                "duration": {
                  "lengthInSeconds": 6420,
                  "displayMinutes": 120
                },
                "title": {
                  "rating": {
                    "code": "TV-G",
                    "description": ""
                  },
                  "storyLine": {
                    "long": "Fanciful musical biography of wild West sharpshooter Annie Oakley.",
                    "short": "Fanciful musical biography of wild West sharpshooter Annie Oakley."
                  },
                  "releaseYear": 1950,
                  "keywords": "Carnivals, Contests, Costumes, Cowboy hats, Cowboys, Headdresses, Jealousy, Marriage proposals, Native Americans, Rifles, Sharpshooting, Singers, Singing, Theatrical producers & directors, Theatrical productions, Tribal chiefs",
                  "episode": {},
                  "series": {
                    "id": 0
                  },
                  "season": {
                    "name": "Season 0",
                    "number": 0
                  },
                  "titleIds": [
                    {
                      "type": "Feature Film",
                      "value": "1200",
                      "authority": "Turner"
                    },
                    {
                      "type": "Feature Film",
                      "value": "MV000188740000",
                      "authority": "TMS"
                    }
                  ]
                },
                "flights": [
                  {
                    "start": "2016-06-10T10:00:00Z",
                    "end": "2016-07-15T09:59:59Z",
                    "destinations": [
                      {
                        "externalId": 2,
                        "name": "CIM",
                        "properties": [],
                        "deliverables": [] 
                      }
                    ],
                    "tags": []
                  }
                ],
                "flags": {
                  "hd": false,
                  "cx": false,
                  "programmerBrandingReq": false,
                  "fastForwardAllowed": true,
                  "manuallyProcess": false
                },
                "releasedOn": "2016-06-12T10:13:50.767Z",
                "releasedBy": "Linear Promo Refresh"
              }
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []
    
    
    
## Send airing to queues [/v1/airing/send/{airingId}]

### POST[POST]
    
    This operation will send the indicated airing to its respective queues/destinations. During normal delivery workflow the PostOffice job verifies that a particular asset honors the queue criteria and enabled options before
    sending it to that queue. However, with this operation the PostOffice job bypasses all such validations before sending to its respective destinations/queues that are active.
  
    The final delivery of the airing depends on how busy the PostOffice job is.
    
+ Parameters

    + airingId:    CART1003181500015319 (required, string) - airing to be sent
   

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json

+ Response 200 (application/json)

    + Body
            
            <!-- success -->
            {
              "airingId": "CART1003181500015319",
              "message": "AiringId flagged for delivery for valid queues"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []
    
+ Response 404 (application/json)

    + Body
    
              {
              "message": "airingId not found"
              }
         
          
## Send airings to specific queue [/v1/airing/send]

### POST[POST]
    
    This operation will send one or more airings to the specified queue. With this operation the PostOffice job will bypass queue criteria validation but will honor enabled queue options before sending the airings to the specificed active queue.
  
    The final delivery of the airing depends on how busy the PostOffice job is.


+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
    + Body
    
            {
                "QueueName" : "d2db802c-64c2-47e8-9612-450ad6c1e853", <!-- unique id/name of the queue-->
                "AiringIds" : ["CARE1007231500005897","CARE1007301500006034"] <!-- list of airings to be sent -->
            }

+ Response 200 (application/json)

    + Body
            
            <!-- success -->
            {
              "validAiringIds": "CART1003181500015319",
              "InvalidAiringIds": "",
              "message": "validAiringIds are flagged for delivery and delivered based on enabled options of the queue"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []
             
+ Response 404 (application/json)

    + Body
    
              {
              "message": "Queue not found"
              }
             
+ Response 412 (application/json)

    + Body
    
              {
              "message": "Inactive queue"
              }          
                       
             
## Submit airing information into ODT [/v1/airing/{prefix}]

### POST[POST]
    
    This operation is based on the permission assigned to the API key and is perhaps the most important route under the Airing resource. Succintly, this operation will:
        
        * create a new airing if the airing ID is not provided in the payload, or
        * update existing airing if the provided airing ID exist, or
        * create a new airing if the provided airing ID doesn't exist
        
    Another subtle feature that this route offers is the concept of submitting airing information by product or destination, but not both. See sample payload for details.
    
    If a new airing is created or an existing airing is updated, its status will be propogated to the reporting system and all queues subscribed to receive airing notification will be informed. 
        
    Required data points as part of JSON request payload:
    
        * Brand
        * Flight window
        * ReleasedBy
        * product or destination 
    
    Validations:
        
        * cannot use previously deleted airing
        * cannot provide both destination and product
        

+ Parameters
    
    + prefix: CARE (string) - Cartoon network string prefix to create new airing ID in the case of scenario 1

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
            
    + Body
    
            <!-- Full payload that can be submitted as part of the request. Not all data points are required, however, the more information you provide, the better it. See description above -->
            {
              "AiringId": "",
              "Name": "",
              "Type": "",
              "Brand": "",
              "Platform": "",
              "Airings": [
                {
                  "Date": "2000-01-01",
                  "AiringId": 0,
                  "Linked": false,
                  "Authority":"Turner"
                }
              ],
              "Duration": {
                "LengthInSeconds": 0,
                "DisplayMinutes": 0
              },
              "Title": {
                "Rating": {
                  "Code": "",
                  "Description": ""
                },
                "StoryLine": {
                  "Long": "",
                  "Short": ""
                },
                "ReleaseYear": 0000,
                "Keywords": "",
                "OriginalPremiereDate": "0001-01-01T00:00:00",
                "Episode": {
                  "Name": "",
                  "Number": "",
                  "ProductionNumber": ""
                },
                "Element": {
                  "Name": "",
                  "Number": "",
                  "ProductionNumber": ""
                },
                "Series": {
                  "Name": "",
                  "Id": 0
                },
                "Season": {
                  "Name": "",
                  "Number": 0
                },
                "Participants": [
                  {
                    "Name":"",
                    "Role": ""
                  }
                ],
                "TitleIds": [
                  {
                    "Type":"",
                    "Value":"",
                    "Authority": "",
                    "Primary": false
                  }
                ]
              },
              "Flights": [
                {
                  "Start": "0001-01-01T00:00:00",
                  "End": "0001-01-01T00:00:00",
                  "Destinations": [  <!-- If you wish to submit airing by destination then populate this property -->
                    {
                      "ExternalId":0,
                      "Name": "",
                      "AuthenticationRequired": false,
                      "Package":{
                        "PackageName":"",
                        "FileName": "",
                        "TitleDigital":"",
                        "TitleBrief":"",
                        "Genres":["",""],
                        "SubGenres":["",""],
                        "ContentTiers":["",""],
                        "ProductCodes":["",""],
                        "GuideCategories":["",""],
                        "ProgramTypes":["",""],
                        "Categories":["",""]
                      }
                    }
                  ],
                  "Product":[ <!-- If you wish to submit airing by products then populate this property -->
                    {
                          "ExternalId":"",  <!-- Provide a valid GUID as string -->
                          "isAuth": false <!-- Will map to destinations for this product. If the same destination is on multiple products, the least restrictive will win -->
                    }
                  ],
                  "Tags":["",""]
                }
              ],
              "Flags": {
                "Hd": false,
                "Cx": false,
                "ProgrammerBrandingReq": false,
                "FastForwardAllowed": false,
                "ManuallyProcess": false,
                "stacked": false
              },
              "Turniverse": {
                "Start":"0001-01-01T00:00:00",
                "End":"0001-01-01T00:00:00",
                "Feeds":[
                  {
                   "Name": ""
                  }
                ]
              },
              "Versions": [
                {
                 "Source": "",
                 "ContentId": "",
                 "ClosedCaptioning":{
                      "File":"",
                      "Encode":""
                  }
                }
              ],
              "PlayList": [
                {
                  "Position":0,
                  "Id":"",
                  "Type": "",
                  "ItemType": "",
                  "IdType": ""
                }
              ],
              "DeviceExclusions": [
                "",""
              ],
              "WebFlags": [
                "",""
              ],
              "ReleaseId": "00000000-0000-0000-0000-000000000000",
              "ReleasedBy": "",
              "Instructions": {
                "DeliverImmediately": false,  <!-- true:indicates that this airing should bypass PostOffice's normal validation and send immediately to its destination/queues -->
                "DisableTracking": false
              },
              "Properties": { 
                    <!-- can include one or more key/value pairs of varying data types -->
              }
            }

+ Response 200 (application/json)

    + Body
            
            
            <!-- sample response after a new cartoon network asset is created -->
            {
              "airingId": "CARE1007201600000206",
              "mediaId": "3ad091fcee8aec2c65d83f2eb7adf59fbde808ad",
              "name": "ISCI",
              "brand": "Cartoon",
              "platform": "Cable",
              "airings": [{
                  "date": "2000-01-01T05:00:00Z",
                  "airingId": 0,
                  "linked": false,
                  "authority":"Turner"}],
              "duration": {
                "lengthInSeconds": 23123,
                "displayMinutes": 0
              },
              "title": {
                "rating": {},
                "storyLine": {},
                "releaseYear": 0,
                "originalPremiereDate": "0001-01-01T00:00:00",
                "episode": {},
                "series": {},
                "season": {
                  "number": 0
                },
                "participants": [],
                "titleIds": []
              },
              "flights": [
                {
                  "start": "2015-01-01T05:00:00Z",
                  "end": "2015-01-02T05:00:00Z",
                  "destinations": [
                    {
                      "externalId": 0,
                      "name": "TVN",
                      "authenticationRequired": false,
                      "package": {
                        "genres": [],
                        "subGenres": [],
                        "contentTiers": [],
                        "productCodes": [],
                        "guideCategories": [],
                        "programTypes": [],
                        "categories": []
                    },
                    {
                      "externalId": 0,
                      "name": "Canoe",
                      "authenticationRequired": false,
                      "package": {
                        "genres": [],
                        "subGenres": [],
                        "contentTiers": [],
                        "productCodes": [],
                        "guideCategories": [],
                        "programTypes": [],
                        "categories": []
                      }
                    }
                  ],
                  "tags": []
                }
              ],
              "flags": {
                "hd": true,
                "cx": false,
                "programmerBrandingReq": false,
                "fastForwardAllowed": false,
                "manuallyProcess": false,
                "stacked": false
              },
              "versions": [
                {
                  "contentId": "321332",
                  "closedCaptioning": {
                    "file": false,
                    "encode": false
                  }
                }
              ],
              "playList": [],
              "deviceExclusions": [],
              "webFlags": [],
              "releasedOn": "2016-07-20T16:52:04.516Z",
              "releasedBy": "ntuser",
              "options": {
                "files": [],
                "titles": [],
                "series": [],
                "changes": [],
                "destinations": [],
                "packages": []
              },
              "properties": {
                "key1": "value1",
                "key2": 5.299,
                "key3": false
              }
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []
             

## Delete airing [/v1/airing]

### DELETE[DELETE]
    
    This operation is based on the permission assigned to the API key. It will delete the indicated airing if it exist. If deletion is successful then all of the subscribed queues will be notified.
 
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
            
    + Body
    
            
            {
                "AiringId": "TBSD1002191501000010"  <!-- required -->
                "ReleasedBy": "ntuser" <!--required -->
            }

+ Response 200 (application/json)

    + Body
            
            
            {
                "airingId": "TBSD1002191501000010",
                "message": "deleted sucessfully"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []
    
            <!-- Possible cause:
                * this doesn't have a network/brand associated with it
            -->
            {
              "message": "Request denied for TBSD1002191501000010 brand.",
              "exception": "..."
            }
            
            <!-- Possible cause:
                * this doesn't have any flight/destination information
            -->
            {
              "message": "Request denied for TBSD1002191501000010 airing.",
              "exception": "..."
            }
            
# Group Airing Status Resource

Routes and operations related to airing status resource

## Submit Airing workflow status information into ODT [/v1/airingstatus/{airingid}]

### POST[POST]
    
    This operation is based on permission assigned to the API key. Succintly, this operation will:
        
        * update the airing with the given status 
        
    Required data points as part of JSON request payload:
    
        * Status. It's a key/value pair, where key is the [name of the status] and value is the [state of the status]. Here's an example: 
               "Status":  
                {  
                    "MEDIUM":"true",
                    "ENCODING": "false"
                }
                
                The above example indicates that the airing has 'MEDIUM' status set to true and 'ENCODING' status set to false. From a business prespective it can inferred that medium service completed their airing work flow tasks, while encoding hasn't. Please refer to ODT admin portal for the full list of statuses.
    
    Validations:
       
        * cannot use custom key (name), it should exist in ODT
        * allowed value (state) for a key (name) is true or false. Cannot use null or any other values
        * cannot update status of previously deleted airing
        * cannot update status of an expired airing
         

+ Parameters
    
    + airingid: CARE1002091700019509 (string) - any current airing id

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
            
    + Body
    
            <!-- Payload that can be submitted as part of the request. See description above -->
            {  
              "Status":  
                  {  
                     "MEDIUM":"true",
                     "ENCODING": "false"
                  }
            }

+ Response 200 (application/json)

    + Body
            
          
            {
                "result": "Successfully updated the airing status."
            }    

            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []

+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []

            
+ Response 404 (application/json)

    + Body
                
            {
              "Provided AiringID does not exists or expired."
            }
            
## Submit Airing workflow status information into ODT using MediaId [/v1/airingstatus/mediaId/{mediaid}]

### POST[POST]
    
    This operation is based on permission assigned to the API key. Succintly, this operation will:
        
        * update all active airings belonging to the provided mediaId with the given statuses 
        
    Required data points as part of JSON request payload:
    
        * Status. It's a key/value pair, where key is the [name of the status] and value is the [state of the status]. Here's an example: 
               "Status":  
                {  
                    "MEDIUM":"true",
                    "ENCODING": "false"
                }
                
                The above example indicates that the airing has 'MEDIUM' status set to true and 'ENCODING' status set to false. From a business prespective it can inferred that medium service completed their airing work flow tasks, while encoding hasn't. Please refer to ODT admin portal for the full list of statuses.
    
    Validations:
       
        * cannot use custom key (name), it should exist in ODT
        * allowed value (state) for a key (name) is true or false. Cannot use null or any other values
        * cannot update status of previously deleted airing
        * cannot update status of an expired airing
         

+ Parameters
    
    + mediaid: 5e61a74016f1d903adc5b1d4ca5425a5b575afe7 (string) - any current media id

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
            
    + Body
    
            <!-- Payload that can be submitted as part of the request. See description above -->
            {  
              "Status":  
                  {  
                     "MEDIUM":"true",
                     "ENCODING": "false"
                  }
            }

+ Response 200 (application/json)

    + Body
            
          
            {
                "result": "Successfully updated the airings status."
            }    

            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []

+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []

            
+ Response 404 (application/json)

    + Body
                
            {
              "Provided MediaID does not exists or expired."
            }            
                     
      
# Group Playlist Resource

Routes and operations related to playlist resource

## Submit Playlist information into ODT [/v1/playlist/{airingid}]

### POST[POST]
    
    This operation is based on the permission assigned to the API key. Succintly, this operation will:
        
        * update existing playlist for the provided airing ID 
        
    Required data points as part of JSON request payload:
    
        * PlayList
        * ReleasedBy
    
    Validations:
        
        * cannot use previously deleted airing
        * cannot use expired airing
        * cannot use new Segment CIDs other than associated version CIDs
        

+ Parameters
    
    + airingid: CARE1002091700019509 (string) - any current airing id

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
            
    + Body
    
            <!-- Full payload that can be submitted as part of the request. Not all data points are required, however, the more information you provide, the better it. See description above -->
            {  
               "PlayList":[  
                  {  
                     "Position":1,
                     "Id":"1E66T/01",
                     "Type":"MaterialID",
                     "ItemType":"Segment",
                     "IdType":"MaterialID"
                  },
                  {  
                     "Position":2,
                     "Id":"",
                     "Type":"Trigger",
                     "ItemType":"Trigger",
                     "IdType":""
                  },
                  {  
                     "Position":3,
                     "Id":"1E66T/02",
                     "Type":"MaterialID",
                     "ItemType":"Segment",
                     "IdType":"MaterialID"
                  },
                  {  
                     "Position":4,
                     "Id":"",
                     "Type":"Trigger",
                     "ItemType":"Trigger",
                     "IdType":""
                  },
                  {  
                     "Position":5,
                     "Id":"1E66T/03",
                     "Type":"MaterialID",
                     "ItemType":"Segment",
                     "IdType":"MaterialID"
                  },
                  {  
                     "Position":6,
                     "Id":"",
                     "Type":"Trigger",
                     "ItemType":"Trigger",
                     "IdType":""
                  },
                  {  
                     "Position":7,
                     "Id":"1E66T/04",
                     "Type":"MaterialID",
                     "ItemType":"Segment",
                     "IdType":"MaterialID"
                  },
                  {  
                     "Position":8,
                     "Id":"",
                     "Type":"Trigger",
                     "ItemType":"Trigger",
                     "IdType":""
                  },
                  {  
                     "Position":9,
                     "Id":"1E66T/05",
                     "Type":"MaterialID",
                     "ItemType":"Segment",
                     "IdType":"MaterialID"
                  }
               ],
               "ReleasedBy":"ODT"
            }

+ Response 200 (application/json)

    + Body
            
            
            <!-- sample response after a new cartoon network asset is created -->
            Successfully updated the playlist.     

            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []

+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the right privileges-->
             []

             
# Group Airing ID Resource

Routes and operations related to airing ID resource

## Generate airing ID [/v1/airingId/generate/{prefix}]

### GET[GET]
    
    Generate a new empty airing with ID prepended by the provided prefix
 
 
+ Parameters

    + prefix: TBSE (string, required) - prefix that will be prepended to the generated airing ID

                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
            
            {
              "id": "566ad73f7bebff0dfc9b21c7",
              "airingId": "TBSE1007201600000037",
              "prefix": "TBSE",
              "sequenceNumber": 37,
              "billingNumberLower": 1,
              "billingNumberCurrent": 37,
              "billingNumberUpper": 99999,
              "createdBy": "",
              "createdDateTime": "2016-07-20T18:11:49.4779705Z",
              "modifiedBy": "",
              "modifiedDateTime": "2016-07-20T18:11:49.4798045Z"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []
           
           
## Generate airing ID [/v1/airingId/{prefix}]

### POST[POST]
    
    Generate a new empty airing with ID prepended by the provided prefix. Same as the previous GET operation.
 
 
+ Parameters

    + prefix: TBSE (string, required) - prefix that will be prepended to the generated airing ID

                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
            
            {
              "id": "578fc17935f3861950be1c0e",
              "airingId": "TBSE1007201600000001",
              "prefix": "TBSE",
              "sequenceNumber": 1,
              "billingNumberLower": 0,
              "billingNumberCurrent": 0,
              "billingNumberUpper": 0,
              "createdBy": "",
              "createdDateTime": "2016-07-20T18:22:49.2491036Z",
              "modifiedBy": "",
              "modifiedDateTime": "2016-07-20T18:22:49.2491036Z"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []
             
             
             
# Group Destination Resource

Routes and operations related to destination resource

## Retrieve destination details [/v1/destination/{name}]

### GET[GET]
    
    Retrieve destination that match the given name. If there are mutliple matches then return the first one.
 
 
+ Parameters

    + name: CMA (string, required) - destination name

                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
            
            {
              "externalId": 0,
              "name": "CMA",
              "description": "Internal",
              "properties": [
                {
                  "name": "Description",
                  "value": "Internal"
                },
                {
                  "name": "IP Address",
                  "value": "12.109.233.27"
                },
                {
                  "name": "Username",
                  "value": "Test1"
                },
                {
                  "name": "Password",
                  "value": "123"
                },
                {
                  "name": "File Count",
                  "value": "45"
                }
              ],
              "deliverables": [
                {
                  "value": "{AIRING_ID}.XML"
                },
                {
                  "value": "{AIRING_ID}.mp4"
                },
                {
                  "value": "{AIRING_ID}HLS"
                }
              ],
              "content": {
                "highDefinition": true,
                "standardDefinition": true,
                "cx": true,
                "nonCx": true
              },
              "playlists": [],
              "auditDelivery": true,
              "createdBy": "c-pyramid-psharma",
              "createdDateTime": "2012-08-02T11:46:33.28Z",
              "modifiedBy": "c-xpanxion-ganesan",
              "modifiedDateTime": "2016-07-13T07:25:26.64Z"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []
             
            <!-- your API key doesn't have permission to this destination-->
            {
              "message": "Request denied for CMA destination.",
              "exception": "..."
            }
                       
           
## Retrieve all destinations [/v1/destinations]

### GET[GET]
    
    Retrieve all existing destinations that your API key is authorized to view.

                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
    
            <!-- no authorized destinations -->
            []
            
            <!-- authorized destinations -->
            [
                 {
                    "externalId": 17,
                    "name": "EXT",
                    "description": "External",
                    "properties": [
                      {
                        "name": "Description",
                        "value": "External"
                      },
                      {
                        "name": "IP Address",
                        "value": ""
                      },
                      {
                        "name": "Username",
                        "value": ""
                      },
                      {
                        "name": "Password",
                        "value": ""
                      },
                      {
                        "name": "File Count",
                        "value": ""
                      }
                    ],
                    "deliverables": [],
                    "content": {
                      "highDefinition": true,
                      "standardDefinition": true,
                      "cx": true,
                      "nonCx": true
                    },
                    "playlists": [],
                    "auditDelivery": false,
                    "createdBy": "c-pyramid-psharma",
                    "createdDateTime": "2012-08-02T11:46:33.287Z",
                    "modifiedBy": "kendavis",
                    "modifiedDateTime": "2015-10-05T21:35:23.471Z"
                },
                {
                    "externalId": 19,
                    "name": "TLTN ",
                    "description": "TeleToons",
                    "properties": [
                      {
                        "name": "Description",
                        "value": "TeleToons"
                      },
                      {
                        "name": "IP Address",
                        "value": ""
                      },
                      {
                        "name": "Username",
                        "value": ""
                      },
                      {
                        "name": "Password",
                        "value": ""
                      },
                      {
                        "name": "File Count",
                        "value": ""
                      }
                    ],
                    "deliverables": [],
                    "content": {
                      "highDefinition": true,
                      "standardDefinition": true,
                      "cx": true,
                      "nonCx": true
                    },
                    "playlists": [],
                    "auditDelivery": false,
                    "createdBy": "c-pyramid-psharma",
                    "createdDateTime": "2012-08-02T11:46:33.287Z",
                    "modifiedBy": "kendavis",
                    "modifiedDateTime": "2015-10-05T21:35:23.583Z"
              }
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []

# Group Product Resource

Routes and operations related to product resource

## Retrieve product details [/v1/products]

### GET[GET]
    
    Retrieve all products defined in ODT
 

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
            
            [{
                "externalId": "89159cab-59f7-4948-92ce-f82b0c2084e0",
                "name": "(Cartoon) (Broadband) CN.com Auth",
                "description": "AUTHENTICATED",
                "mappingId": 227,
                "dynamicAdTrigger": false,
                "destinations": [
                  "CMA",
                  "CNWB",
                  "ENCOM"
                ],
                "createdBy": "Amaple",
                "createdDateTime": "2010-06-07T16:07:52Z",
                "modifiedBy": "jbulger",
                "modifiedDateTime": "2016-07-12T17:43:33.134Z"
              },
              {
                "externalId": "dab436d6-ba93-4a34-bdaa-f38fcb9a10d6",
                "name": "(Cartoon) (Broadband) CN.com Unauth",
                "description": "Cartoonnetwork.com Unauthenticated",
                "mappingId": 228,
                "dynamicAdTrigger": true,
                "destinations": [
                  "CMA",
                  "CANOE"
                ],
                "createdBy": "Amaple",
                "createdDateTime": "2010-06-07T16:11:09Z",
                "modifiedBy": "c-xpanxion-ganesan",
                "modifiedDateTime": "2016-07-19T09:57:29.884Z"
              }
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
            []
           

## Retrieve destinations by product [/v1/product/{productId}/destinations]

### GET[GET]
    
    Retrieve destinations mapped to the given product

+ Parameters

    + productId: 89159cab-59f7-4948-92ce-f82b0c2084e0 (string, required) - unique product id as defined in ODT
    

+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
            
            [{
                "externalId": 0,
                "name": "CMA",
                "description": "Internal",
                "properties": [
                  {
                    "name": "Description",
                    "value": "Internal"
                  },
                  {
                    "name": "IP Address",
                    "value": "12.109.233.27"
                  },
                  {
                    "name": "Username",
                    "value": "Test1"
                  },
                  {
                    "name": "Password",
                    "value": "123"
                  },
                  {
                    "name": "File Count",
                    "value": "45"
                  }
                ],
                "deliverables": [
                  {
                    "value": "{AIRING_ID}.XML"
                  },
                  {
                    "value": "{AIRING_ID}.mp4"
                  },
                  {
                    "value": "{AIRING_ID}HLS"
                  }
                ],
                "content": {
                  "highDefinition": true,
                  "standardDefinition": true,
                  "cx": true,
                  "nonCx": true
                },
                "playlists": [],
                "auditDelivery": true,
                "createdBy": "c-pyramid-psharma",
                "createdDateTime": "2012-08-02T11:46:33.28Z",
                "modifiedBy": "c-xpanxion-ganesan",
                "modifiedDateTime": "2016-07-13T07:25:26.64Z"
              },
              {
                "externalId": 60,
                "name": "CNWB",
                "description": "Cartoon Network Web Site",
                "properties": [],
                "deliverables": [],
                "content": {
                  "highDefinition": true,
                  "standardDefinition": true,
                  "cx": true,
                  "nonCx": true
                },
                "playlists": [],
                "auditDelivery": false,
                "createdBy": "jbulger",
                "createdDateTime": "2015-11-10T14:54:46.661Z",
                "modifiedBy": "c-xpanxion-ranandan",
                "modifiedDateTime": "2016-05-05T09:46:06.876Z"
              }
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- Your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- Your API key doesn't have right privileges-->
             []
             
# Group File Resource

Routes and operations related to files resource

## Retrieve files by title ID [/v1/files/title/{titleId}]

### GET[GET]
    
    Retrieve all files (image, video etc) that match the title ID.
 
 
+ Parameters

    + titleId: 2056506 (number, required) - title ID

                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
    
            <!-- no matching files -->
            []

            <!-- files (images) that match the given title ID-->
            [
              {
                "titleId": 2056506,
                "type": "jpg",
                "path": "/tnt/images/",
                "name": "thumbnail.jpg",
                "domain": "cdn8view.turner.com",
                "category": "thumbnails",
                "secure": false,
                "video": false
              },
              {
                "titleId": 2056506,
                "type": "jpg",
                "path": "/tnt/images/1",
                "name": "thumbnail.jpg",
                "domain": "cdn8view.turner.com",
                "category": "thumbnails",
                "secure": false,
                "video": false
              }
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []
                 
           
## Retrieve files by airing ID [/v1/files/airing/{airingId}]

### GET[GET]
    
    Retrieve all files (image, video etc) that match the airing ID.

+ Parameters

    + airingId: CARE1004061600009318 (string, required) - airing ID

                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json


+ Response 200 (application/json)

    + Body
    
            <!-- No matching files -->
            []
            
            <!-- Matching files (image and video) -->
            [
                {
                  "mediaId": "b000cd3ffb9676bd307b9bf5c4a7fff4da375669",
                  "airingId": "TBSE1004201600030495",
                  "contents": [
                    {
                      "name": "advanced_hls",
                      "media": [
                        {
                          "type": "advanced_hls",
                          "totalDuration": 1290.9243,
                          "contentSegments": [
                            {
                              "segmentIdx": 0,
                              "start": 0,
                              "duration": 595.5623
                            },
                            {
                              "segmentIdx": 1,
                              "start": 595.5953,
                              "duration": 695.3621
                            }
                          ],
                          "playlists": [
                            {
                              "name": "master",
                              "type": "hls",
                              "urls": [
                                {
                                  "bucketUrl": {
                                    "host": "http://public-origin-edc.s3.amazonaws.com",
                                    "path": "/3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c0000041f/",
                                    "fileName": "master.m3u8"
                                  }
                                },
                                {
                                  "akamaiUrl": {
                                    "host": "http://edc-test.cdn.turner.com",
                                    "path": "/3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c0000041f/",
                                    "fileName": "master.m3u8"
                                  }
                                }
                              ],
                              "properties": {
                                "isAVMUX": false,
                                "mediaContainer": "ts",
                                "encryption": "sample-aes",
                                "drmAccess": true,
                                "drmWideVine": true,
                                "drmFairPlay": true,
                                "drmClearKey": true,
                                "assetId": "3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c0000041f",
                                "protectionType": "spe"
                              }
                            },
                            {
                              "name": "master_de",
                              "type": "hls",
                              "urls": [
                                {
                                  "bucketUrl": {
                                    "host": "http://public-origin-edc.s3.amazonaws.com",
                                    "path": "/3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c0000041f/",
                                    "fileName": "master_de.m3u8"
                                  }
                                },
                                {
                                  "akamaiUrl": {
                                    "host": "http://edc-test.cdn.turner.com",
                                    "path": "/3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c0000041f/",
                                    "fileName": "master_de.m3u8"
                                  }
                                }
                              ],
                              "properties": {
                                "isAVMUX": false,
                                "mediaContainer": "ts",
                                "encryption": "sample-aes",
                                "drmAccess": true,
                                "drmWideVine": true,
                                "drmFairPlay": true,
                                "drmClearKey": true,
                                "assetId": "3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c0000041f",
                                "protectionType": "spe"
                              }
                            }
                          ],
                          "adType": "c4",
                          "captions": [
                            {
                              "type": "608",
                              "location": "cc1",
                              "language": "eng"
                            },
                            {
                              "type": "708",
                              "location": "svc1",
                              "language": "eng"
                            }
                          ]
                        }
                      ]
                    }
                  ]
                },
                {
                  "airingId": "TBSE1004201600030495",
                  "type": "mp4",
                  "path": "/tnt/video/",
                  "name": "CARE1007231500005863.mp4",
                  "domain": "cdn8view.turner.com",
                  "category": "gadget",
                  "secure": false,
                  "video": false
                } 
            ]
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []


+ Response 404 (application/json)

    + Body
                
            {
              "message": "Airing with id CARE1004061600009318 does not exist in collection.",
              "exception": "..."
            }
            
            

## Submit files [/v1/files]

### POST[POST]
    
    Within ODT, files can be categorized into two - video and non-video files. This operation allows users to submit both video and non-video files. Video and non-video payloads are drastically different. See request/body section for sample payload. The key data point that allows this operation to distinguish between video & non-video payload is this data point - "video":true/false.

    Non-video files can ONLY be registered by title ID. Video files can be registered by MediaID, TitleID or AiringID, but not multiple. Both failure and success of this operation will be notified to the reporting system. 
     
    
    Validations:
        * cannot register files (video and non-video) for deleted or expired airings
       
        
        
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
    + Body
    
            <!-- Non-video file payload. Multiple files can be submitted  -->
            [ { 
                  "TitleId": 2004637, 
                  "Path": "/tnt/images/", 
                  "Name": "thumbnail.jpg", 
                  "Type": "jpg", 
                  "Video" : false,  
                  "Domain" : "cdn8view.turner.com", 
                  "Unique" : true,              <!-- If true, the existing file information ( match by TitleId & Match) will be updated; else, add new file entry -->
                  "AspectRatio":"270x229",
                  "Match":"xyz",         <!-- Some value that will uniquely identify this non-video file from the consumer's prespective --> 
                  "Category" : "gadget"
              }]
              
              
              <!-- Video file payload for mediaid. Multiple files can be submitted  -->
              [  {
                  "video": true,
                  "mediaid": "c79d510d02de89fb2cb2520bcf525f814657205c", <!-- Provided mediaid should exist -->
                  "contents": [ <!-- The number of content entries in this array should match the number of versions this media (any airings within it) has -->
                      {
                          "name": "1770054", <!-- If this content already exists for this mediaid, it will be updated; else it will be inserted as a new entry -->
                          "media": [
                              {
                                  "type": "clip", 
                                  "totalDuration": 50, <!-- Greater than or equal to zero -->
                                  "contentSegments": [ 
                                      {
                                          "segmentIdx": 0, <!-- Greater than or equal to zero -->
                                          "start": 0, <!-- Greater than or equal to zero -->
                                          "duration": 50 <!-- Greater than or equal to zero -->
                                      }
                                  ],
                                  "playlists": [ 
                                      {
                                          "name": "master", 
                                          "type": "m3u8", 
                                          "properties": {   <!-- At least one DRM ('drmUnprotected', 'drmWideVine', 'drmFairPlay') key/value pair is required. -->
                                                "drmUnprotected":"true"
                                          },
                                          "urls": [ <!-- Any number of urls can be provided. However, there should be one 'akamaiURL' entry. Each url entry should have 'host', 'path' & 'filename' -->
                                              { 
                                                  "akamaiUrl": {
                                                      "host": "http://tbs-vh.akamaihd.net",
                                                      "path": "/i/samantha-bee/2016/02/09/FFSB1001webcoldopen_281829_,416x240_400,640x360_800,640x360_1400,848x480_1700,960x540_2500,1280x720_3500,.mp4.csmil/",
                                                      "fileName": "master.m3u8"
                                                  }
                                              },
                                              { 
                                                  "turnerUrl": {
                                                      "host": "http://cdn8turner.com",
                                                      "path": "/tnt/xxx/",
                                                      "fileName": "teletoons.mp3"
                                                  }
                                              }  
                                              
                                          ]
                                      }
                                  ],
                                  "captions":[
                                      {
                                          "type": "",
                                          "location": "",
                                          "language":""
                                      }
                                  ]
                              }
                          ]
                      }
                  ]
              }]
              
              
              <!-- Video file payload for airingid. Multiple files can be submitted  -->
              [  {
                  "video": true,
                  "airingId": "CARE1005021600011137", <!-- Provided airing should exist -->
                  "contents": [ <!-- The number of content entries in this array should match the number of versions this media (any airings within it) has -->
                      {
                          "name": "1770054", <!-- If this content already exists for this mediaid, it will be updated; else it will be inserted as a new entry -->
                          "media": [
                              {
                                  "type": "clip", 
                                  "totalDuration": 50, <!-- Greater than or equal to zero -->
                                  "contentSegments": [ 
                                      {
                                          "segmentIdx": 0, <!-- Greater than or equal to zero -->
                                          "start": 0, <!-- Greater than or equal to zero -->
                                          "duration": 50 <!-- Greater than or equal to zero -->
                                      }
                                  ],
                                  "playlists": [ 
                                      {
                                          "name": "master", 
                                          "type": "m3u8", 
                                          "properties": {   <!-- At least one DRM ('drmUnprotected', 'drmWideVine', 'drmFairPlay') key/value pair is required. -->
                                                "drmUnprotected":"true"
                                          },
                                          "urls": [ <!-- Any number of urls can be provided. However, there should be one 'akamaiURL' entry. Each url entry should have 'host', 'path' & 'filename' -->
                                              { 
                                                  "akamaiUrl": {
                                                      "host": "http://tbs-vh.akamaihd.net",
                                                      "path": "/i/samantha-bee/2016/02/09/FFSB1001webcoldopen_281829_,416x240_400,640x360_800,640x360_1400,848x480_1700,960x540_2500,1280x720_3500,.mp4.csmil/",
                                                      "fileName": "master.m3u8"
                                                  }
                                              },
                                              { 
                                                  "turnerUrl": {
                                                      "host": "http://cdn8turner.com",
                                                      "path": "/tnt/xxx/",
                                                      "fileName": "teletoons.mp3"
                                                  }
                                              }  
                                              
                                          ]
                                      }
                                  ],
                                  "captions":[
                                      {
                                          "type": "",
                                          "location": "",
                                          "language":""
                                      }
                                  ]
                              }
                          ]
                      }
                  ]
              }]
              
              <!-- Video file payload for TitleId. Multiple files can be submitted  -->
              [  {
                  "video": true,
                  "TitleId": 798460, 
                  "contents": [ <!-- The number of content entries in this array should match the number of versions this media (any airings within it) has -->
                      {
                          "name": "1770054", <!-- If this content already exists for this mediaid, it will be updated; else it will be inserted as a new entry -->
                          "media": [
                              {
                                  "type": "clip", 
                                  "totalDuration": 50, <!-- Greater than or equal to zero -->
                                  "contentSegments": [ 
                                      {
                                          "segmentIdx": 0, <!-- Greater than or equal to zero -->
                                          "start": 0, <!-- Greater than or equal to zero -->
                                          "duration": 50 <!-- Greater than or equal to zero -->
                                      }
                                  ],
                                  "playlists": [ 
                                      {
                                          "name": "master", 
                                          "type": "m3u8", 
                                          "properties": {   <!-- At least one DRM ('drmUnprotected', 'drmWideVine', 'drmFairPlay') key/value pair is required. -->
                                                "drmUnprotected":"true"
                                          },
                                          "urls": [ <!-- Any number of urls can be provided. However, there should be one 'akamaiURL' entry. Each url entry should have 'host', 'path' & 'filename' -->
                                              { 
                                                  "akamaiUrl": {
                                                      "host": "http://tbs-vh.akamaihd.net",
                                                      "path": "/i/samantha-bee/2016/02/09/FFSB1001webcoldopen_281829_,416x240_400,640x360_800,640x360_1400,848x480_1700,960x540_2500,1280x720_3500,.mp4.csmil/",
                                                      "fileName": "master.m3u8"
                                                  }
                                              },
                                              { 
                                                  "turnerUrl": {
                                                      "host": "http://cdn8turner.com",
                                                      "path": "/tnt/xxx/",
                                                      "fileName": "teletoons.mp3"
                                                  }
                                              }  
                                              
                                          ]
                                      }
                                  ],
                                  "captions":[
                                      {
                                          "type": "",
                                          "location": "",
                                          "language":""
                                      }
                                  ]
                              }
                          ]
                      }
                  ]
              }]
              
+ Response 200 (application/json)

    + Body
    
            {
              "result": "success"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have right privileges-->
             []
             
# Group Package Resource

Routes and operations related to package resource

## Submit packages [/v1/package]

### POST[POST]
    
  This operation allows submission of packaging data into ODT based on airing Id, title Ids or content Ids.See sample payload in request/body section.

  When a package is modified, notification will be sent to the queues based on its criteria.
  

    Required data points as part of JSON request payload:
    
        * AiringId or ContentIds  or TitleIds
        
        * Type
        
        * PackageData
      
    Validations:
     
        * Cannot submit packages in combination of AiringId, ContentIds and TitleIds . Only one can be provided at a time.
        
        * Valid AiringId (non expired or not deleted) should be provided.
        
        * Combination of Id (either airing Id or content Ids or titleIds), Type and DestinationCode should be considered unique when submitting a package.
        
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
    + Body
    
              //Register Package by TitleIds
              //example 1
              {
                "TitleIds": [1234],
                <!-- what kind of package are you giving odt? -->
                "Type": "csc-mezz", 
                "DestinationCode": "ATTB",
                "PackageData": {
                  <!-- put whatever json you want in here -->
                }
              }
              //example 2
              {
                "TitleIds": [1234], 
                <!-- what kind of package are you giving odt? -->
                "Type": "csc-proxy",
                "DestinationCode" : "",
                "PackageData": {
                  <!-- put whatever json you want in here -->
                }
              }
              //example 3
              {
                "TitleIds": [1234, 5678],
                "Type": "csc-mezz", <!-- what kind of package are you giving odt? -->
                "PackageData": {
                  <!-- put whatever json you want in here -->
                }
              }
              
              //Register Package by ContentIds
              //example 1
              { 
                "ContentIds": ["1ANKG"],
                "Type": "csc-mezz", <!-- what kind of package are you giving odt? -->
                "DestinationCode": "ATTB",
                "PackageData": {
                  <!-- put whatever json you want in here -->
                }
              }
              
              //example 2
              { 
                "ContentIds": ["1ANKG","QZZ5"],
                "Type": "csc-proxy", <!-- what kind of package are you giving odt? -->
                "PackageData":{
                  <!-- put whatever json you want in here -->
                }
              }
              
              //Register Package by AiringId
              //example 1
              { 
                "AiringId": "TBSE1002271700003803",  <!-- A valid airing Id -->
                "Type": "csc-mezz", <!-- what kind of package are you giving odt? -->
                "DestinationCode": "ATTB",
                "PackageData": {
                  <!-- put whatever json you want in here -->
                }
              }
              
            

+ Response 200 (application/json)

    + Body
    
            {
              "result": "success"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the correct privileges-->
             []


### DELETE[DELETE]
    
  This operation allows deletion of packaging data from ODT based on matching Id (either TitledIds or ContentIds or AiringID), Type and DestinationCode. Currently, only one Package can be deleted at a time. See sample payload in request/body section.

  When a package is deleted, notification will be sent to the queues based on its criteria.
  
    Validations:
        
        * Cannot submit packages in combination of AiringId, ContentIds and TitleIds . Only one can be provided at a time.
        
        * Valid AiringId should be provided.
        
        * Combination of Id (either AiringId or ContentIds or TitleIds), Type and DestinationCode should be considered unique when submitting a package.
        
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
    + Body
            
            //Delete Package by TitleIds
            //example 1
            {
                "TitleIds": [1234],
                "Type": "csc-mezz", 
                "DestinationCode": "ATTB"
            }
            
            //Delete Package by ContentIds
            //example 1
            {
                "Contentids": ["1E66T"],
                "Type": "csc-mezz", 
                "DestinationCode": "ATTB"
            }
            
            
            //Delete Package by AiringId
            //example 1
            { 
                "AiringId": "TBSE1002271700003803",  <!-- A valid airing Id -->
                "Type": "csc-mezz",
                "DestinationCode": "ATTB"
            }
            

+ Response 200 (application/json)

    + Body
    
            {
              "result": "success"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the correct privileges-->
             []             
                 

# Group Encoding Handler Resource

Routes and operations related to encoding handler resource

## Submit encoding data by media ID [/v1/handler/encoding]

### POST[POST]
    
  This operation allows submission of encoding data into ODT. Currently this payload can ONLY be registered by media ID. See sample payload in request/body section. Both success and failure of this operation will be propogated to the reporting system.
     
                
+ Request JSON Message

    + Headers
            
            Authorization: [insert your API authorization key]
            Content-Type: application/json
            Accept: application/json
            
    + Body
                  
              <!-- encoding payload  -->
              {
                "odt-airing-id": "CART1007121600024562" 
                "odt-media-id": "c79d510d02de89fb2cb2520bcf525f814657205c",
                "root-id": "3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c38",
                "output": [
                  {
                    "output": "advanced_hls",
                    "total-duration": "1275.1100",
                    "adType":"c4",
                    "closed-captions-type":"608",
                    "content-segments": [
                      {
                        "segment-idx": 0,
                        "start": "0.0000",
                        "duration": "387.0210"
                      },
                      {
                        "segment-idx": 1,
                        "start": "387.0540",
                        "duration": "383.0500"
                      },
                      {
                        "segment-idx": 2,
                        "start": "770.1040",
                        "duration": "278.9790"
                      },
                      {
                        "segment-idx": 3,
                        "start": "1049.0830",
                        "duration": "226.0600"
                      }
                    ],
                    "master-playlists": [
                      {
                        "name": "master",
                        "type": "hls",
                        "avmux": false,
                        "media-container": "ts",
                        "encryption": "sample-aes",
                        "drm-access": "true",
                        "drm-widevine": true,
                        "drm-fairplay": true,
                        "drm-clearkey": false,
                        "bucket-url": "http:\/\/public-origin-edc.s3.amazonaws.com\/3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C0000010C\/master.m3u8",
                        "asset-id": "3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C0000010C"
                      },
                      {
                        "name": "master_de",
                        "type": "hls",
                        "avmux": false,
                        "media-container": "ts",
                        "encryption": "sample-aes",
                        "drm-access": true,
                        "drm-widevine": true,
                        "drm-fairplay": true,
                        "drm-clearkey": false,
                        "bucket-url": "http:\/\/public-origin-edc.s3.amazonaws.com\/3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C0000010C\/master_de.m3u8",
                        "asset-id": "3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C0000010C"
                      },
                      {
                        "name": "master_ph",
                        "type": "hls",
                        "avmux": false,
                        "media-container": "ts",
                        "encryption": "sample-aes",
                        "drm-access": true,
                        "drm-widevine": true,
                        "drm-fairplay": true,
                        "drm-clearkey": false,
                        "bucket-url": "http:\/\/public-origin-edc.s3.amazonaws.com\/3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C0000010C\/master_ph.m3u8",
                        "asset-id": "3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C3C0000010C"
                      }
                    ]
                  }
                ]
              }
              
+ Response 200 (application/json)

    + Body
    
            {
              "result": "success"
            }
            
            
+ Response 401 (application/json)

    + Body
    
            <!-- your API key not recognized -->
            []
            
            
                      
+ Response 403 (application/json)

    + Body
    
            <!-- your API key doesn't have the correct privileges-->
             []



# Message Queue

Operations for utilizing your message queue.

## Retrieve a message from your queue

### GET [GET]

Retrieving a message from your queue can be done in many ways. Please consult rabbit mq documentation on the proper way to consume your queue (https://www.rabbitmq.com/getstarted.html). Please note, that as a consumer of an ODT queue, you must **not declare** your queue, but rather simply connect to it and grab messages from it. This is important as depending on the features we set up for you as our customer, the definition of you queue may change over time.
             
    
+ Response for an Airing you may or may not already have

    + Body
    
            {
              "AiringId":"TBSE1004221500027536",
              "Action":"Modify"
            }

+ Response for an Airing you are being told to delete/remove/take down

    + Body
    
            {
              "AiringId":"TBSE1004221500027536",
              "Action":"Delete"
            }
