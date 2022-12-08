from flask import Flask
from send_matrix import Matrix
import json

matrix = Matrix(3, 5, 11)
app = Flask(__name__)


@app.route("/ordened")
def get_ordened():
  ordened_matrix = matrix.ordened_structure()
  return json.dumps(ordened_matrix)


@app.route("/random")
def get_random():
  matrix.random_structure()
  return json.dumps(matrix.current_random_matrix.tolist())


@app.route("/rewards")
def get_rewards():
  rewards = matrix.rewards()
  return json.dumps(rewards)


@app.route("/")
def get_ready():
  return "ready"


if __name__ == "__main__":
  app.run(host="0.0.0.0")
