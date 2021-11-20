const express = require("express");
let util = require("util");

const { exec } = require("child_process");
const csv = require("csv-parser");
const fs = require("fs");


let adviceFile = fs.readFileSync('advice.json');
let advice = JSON.parse(adviceFile)

function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}
function getAdvisoryMessage(type, subtype) {
  
    adviceIndex = getRandomInt(advice[type][subtype].length)
    return advice[type][subtype][adviceIndex]
}

let exec_prom = util.promisify(exec);


const app = express();
const port = 3000;
app.use(express.json());

app.get("/", (req, res) => {
  res.send("Hello World!");
});

app.post("/evaluate/", (req, res) => {
  const message = req.body["message"];
  const analyze = req.body["analyze"];

  console.log("EVALUATING MESSAGE> " + message);

  const results = [];


  if (analyze) {
    try {
      exec_prom(
        '/home/proj/discrimino/toxic-env/bin/python /home/proj/discrimino/detoxify/run_prediction.py --input "' +
          message +
          '" --model_name multilingual --save_to ./results.csv'
      ).then((out) => {
        fs.createReadStream("./results.csv")
          .pipe(csv())
          .on("data", (data) => results.push(data))
          .on("end", () => {
            console.log(results);
            const evaluationParams = results[0];


            let adviceMessage = "Please moderate your message"
            let response = {};
            response["message"] = message;
            response["evaluation_results"] = {
              "toxicity" : evaluationParams["toxicity"],
              "severe_toxicity" : evaluationParams["severe_toxicity"],
              "obscene" : evaluationParams["obscene"],
              "identity_attack" : evaluationParams["identity_attack"],
              "insult" : evaluationParams["insult"],
              "threat" : evaluationParams["threat"],
              "sexual_explicit" : evaluationParams["sexual_explicit"]
            }

            let types = ""

            if (evaluationParams["identity_attack"] > 0.6) {
              console.log("THIS WAS AN IDENDITY ATTACK");
              response["types"] = "[IDENTITY]"
              response["adviceMessage"]  = getAdvisoryMessage("identity","general")
              res.send(response);
              return;
            }
            if (evaluationParams["threat"] > 0.8) {
              console.log("THIS WAS AN EXCESSIVELY THREATENING MESSAGE");
              response["types"] = "[THREAT]"
              response["adviceMessage"]  = getAdvisoryMessage("threat","general")
              res.send(response);
              return;

            }
            if (evaluationParams["obscene"] > 0.8) {
              console.log("THIS WAS AN EXCESSIVELY OBSCENE MESSAGE");
              response["types"] = "[OBSCENE]"
              response["adviceMessage"]  = getAdvisoryMessage("obscene","general")
              res.json(response);
              return;

            }
                        
            else {
              console.log("LIGHT OBSCENE MESSAGE");
              response["types"] = "[LIGHT]"
              response["adviceMessage"] = getAdvisoryMessage("light","general")
              res.send(response);
              return;

            }
          });
      });
    } catch (e) {
      res.error(e);
    }
  }
});



app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});
