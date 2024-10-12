Banco de Dados test
1. Teste Swagger
  Collections:
  1. Lottery - Esta coleção armazena os resultados das loterias, incluindo a data do sorteio, o número do sorteio e o número vencedor.
  
    [
        {
            "_id": "6709e60ae3b1d6c7d987a9d2",
            "drawDate": "2024-10-01T00:00:00Z",
            "drawNumber": 1,
            "winningNumber": 12345
        },
        {
            "_id": "6709f6e2ccefce2fcc265b9e",
            "drawDate": "2024-10-12T00:00:00Z",
            "drawNumber": 2,
            "winningNumber": 75947
        }
    ]
  2. Players - Esta coleção armazena informações sobre os jogadores, incluindo seus números da sorte e a data em que foram recebidos.  

  [{
          "_id": "6709e246e3b1d6c7d987a9d0",
          "Vibes": 7,
          "YourLuckyNumbers": [{
                  "luckyNumber": 62091,
                  "received": "2024-06-18T00:00:00Z"
              }, {
                  "luckyNumber": 94920,
                  "received": "2024-06-25T00:00:00Z"
              }, {
                  "luckyNumber": 27171,
                  "received": "2024-06-26T00:00:00Z"
              }, {
                  "luckyNumber": 75947,
                  "received": "2024-06-26T00:00:00Z"
              }
          ]
      }
  ]
  
2. Teste MongoDb
  1. Collections:
    1. Orders - Esta coleção armazena os pedidos feitos pelos clientes, contendo informações detalhadas sobre os itens comprados, valores e datas.

    [
        {
            "_id": "6709c0de26e4032c3ce45cfc",
            "order_id": "ORD001",
            "customer": {
                "customer_id": "CUST001",
                "name": "João Silva",
                "email": "joao.silva@email.com"
            },
            "items": [
                { 
                  "product_id": "PROD001", 
                  "quantity": 2, 
                  "price": 100.0 
                },
                { 
                  "product_id": "PROD002", 
                  "quantity": 1, 
                  "price": 50.0
                }
            ],
            "total_amount": 250.0,
            "order_date": "2023-08-01T10:00:00Z"
        },
        {
            "_id": "6709c25926e4032c3ce45cfd",
            "order_id": "ORD002",
            "customer": {
                "customer_id": "CUST001",
                "name": "João Silva",
                "email": "joao.silva@email.com"
            },
            "items": [
                { 
                  "product_id": "PROD003", 
                  "quantity": 5, 
                  "price": 10.0
                },
                { 
                  "product_id": "PROD004", 
                  "quantity": 10, 
                  "price": 5.0
                }
            ],
            "total_amount": 100.0,
            "order_date": "2023-08-01T10:00:00Z"
        },
        {
            "_id": "6709c2c626e4032c3ce45cfe",
            "order_id": "ORD003",
            "customer": {
                "customer_id": "CUST001",
                "name": "João Silva",
                "email": "joao.silva@email.com"
            },
            "items": [
                { 
                  "product_id": "PROD005", 
                  "quantity": 2, 
                  "price": 100.0
                },
                { 
                  "product_id": "PROD006", 
                  "quantity": 3, 
                  "price": 50.0"  
                }
            ],
            "total_amount":350.0,
            "order_date": "2023-08-01T10:00:00Z"
        }
    ]

    2. Aggregations:
      1. Agrupa pelo cliente e soma os valores totais das vendas:
        [
            {
                "$group": {                            
                    "_id": "$customer.customer_id",
                    "name": { "$first": "$customer.name" },
                    "total_sales": { "$sum": "$total_amount" }
                }
            }
        ]
        
      2. Separa os arrays "$items", agrupa pelo cliente, soma a quantidade vendida do produto, calcula a receita total e ordena pela quantidade total de forma decrescente:
        [
            { "$unwind": "$items" },
            { 
                "$group": {
                    "_id": "$items.product_id",
                    "total_quantity": { "$sum": "$items.quantity" },
                    "total_revenue": { "$sum": { "$multiply": ["$items.quantity", "$items.price"] } }
                }
            },
            { "$sort": { "total_quantity": -1 } }
        ]
        
      3. Adiciona a propriedade "orderDateConverted", converte texto para data, agrupa pela data convertida (ano/mês/dia), calcula a média do valor total e ordena pela data de forma crescente:
        [
            { 
                "$addFields": {
                    "orderDateConverted": { "$dateFromString": { "dateString": "$order_date" } }
                }
            },
            {
                "$group": {
                    "_id": { "$dateToString": { "format": "%Y-%m-%d", "date": "$orderDateConverted" } },
                    "average_order_value": { "$avg": "$total_amount" }
                }
            },
            { "$sort": { "_id": 1 } }
        ]
        
      4. Adiciona a propriedade "orderDateConverted", converte texto para data, agrupa por ano/mês, soma o total de vendas e a quantidade de ordens, ordenando pelos meses de forma decrescente:
        [
            { 
                "$addFields": {
                    "orderDateConverted": { "$dateFromString": { "dateString": "$order_date" } }
                }
            },
            {
                "$group": {
                    "_id": {
                        "year": { "$year": "$orderDateConverted" },
                        "month": { "$month": "$orderDateConverted" }
                    },
                    "total_sales": { "$sum": "$total_amount" },
                    "order_count": { "$sum": 1 }
                }
            },
            { "$sort": { "_id.year": -1, "_id.month": -1 } }
        ]
