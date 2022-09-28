from flask import Flask
import send_pattern
import send_matrix


app = Flask(__name__)

@app.route("/")
def index():
    p = send_pattern.pattern(5, 3)
    return p


@app.route("/matrix")
def matrix():
    m = send_matrix.Matrix(5, 3)
    return m.structure()
