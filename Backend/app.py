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
    m = send_matrix.Matrix(5, 3, 11)
    #matrix = json.dumps(m._structure().tolist())
    matrix = m._structure().tolist()
    print("matrix: ", matrix, "matrix type: ", type(matrix))
    return matrix
