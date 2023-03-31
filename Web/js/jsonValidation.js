const validVariables = [
  "title",
  "team",
  "start_date",
  "quarters",
  "debug",
  "bg_color_hex",
  "projects"
];
const validProjectVariables = ["name", "label", "date"];

function debounce(func, wait, immediate) {
  let timeout;
  return function () {
    const context = this,
      args = arguments;
    const later = function () {
      timeout = null;
      if (!immediate) func.apply(context, args);
    };
    const callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) func.apply(context, args);
  };
}

function validateJSON() {
  const jsonInput = $("#inputText").val();
  let jsonError = "";
  try {
    const parsedJSON = JSON.parse(jsonInput);
    var start_date = parsedJSON.start_date;
    if (start_date == null) {
      jsonError +=
        'Required variable "start_date" not found! (use sample JSON to help get started}<br>';
    } else {
      if (!isValidDate(start_date)) {
        jsonError +=
          '"start_date" is not a valid format! Please use the format "dd MMM yyyy" example: "01 Jan 2023"<br>';
      }
    }
    var quarters = parsedJSON.quarters;
    if (quarters != null && (isNaN(quarters) || quarters < 1 || quarters > 6)) {
      jsonError +=
        "Please enter a value between 1 and 6 for the number of quarters!<br>";
    }
    var color = parsedJSON.bg_color_hex;
    if (color != null && !isValidHexColor(color)) {
      jsonError += "Invalid value provided for bg_color_hex!<br>";
    }
    var projects = parsedJSON.projects;
    if (projects == null) {
      jsonError +=
        'Required variable "projects" not found! (use sample JSON to help get started}<br>';
    } else {
      if (Array.isArray(projects)) {
        projects.forEach(function (item) {
          found = false;
          if ("date" in item == false) {
            jsonError += "Please provide a date for each project!<br>";
          } else {
            var dateVal = item["date"];
            if (!isValidDate(dateVal)) {
              jsonError +=
                'A project date is using an invalid format! Please use the format "dd MMM yyyy" example: "01 Jan 2023"<br>';
            }
          }
        });
      } else {
        jsonError += '"projects" must be an array of projects!<br>';
      }
    }
    Object.keys(parsedJSON).forEach(function (key) {
      if (!validVariables.includes(key)) {
        jsonError += `Invalid variable name: "${key}"<br>`;
      }
      if (key === "projects") {
        parsedJSON.projects.forEach(function (project) {
          Object.keys(project).forEach(function (projectKey) {
            if (!validProjectVariables.includes(projectKey)) {
              jsonError += `Invalid project variable name: "${projectKey}"<br>`;
            }
          });
        });
      }
    });
  } catch (e) {
    jsonError = e.message;
  }

  if (jsonError === "") {
    try {
      const formattedJSON = JSON.stringify(JSON.parse(jsonInput), null, 4);
      var jsonObj = JSON.parse(jsonInput);
      var color = jsonObj.bg_color_hex;
      if (isValidHexColor(color)) {
        changeBackgroundColor(color);
      } else {
        changeBackgroundColor("#ffffff");
      }
      $("#inputText").val(formattedJSON);
    } catch (e) {
      jsonError = e.message;
    }
  }
  $("#jsonError").html(jsonError);
}

$(document).ready(function () {
  const debouncedValidateJSON = debounce(validateJSON, 500);
  $("#inputText").on("input", debouncedValidateJSON);
});

function changeBackgroundColor(hexColor) {
  const textarea = document.getElementById("inputText");
  textarea.style.backgroundColor = hexColor;
}

function isValidHexColor(str) {
  return /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/.test(str);
}

function isValidDate(dateString) {
  if (!/^\d{2} [a-zA-Z]{3} \d{4}$/.test(dateString)) {
    return false;
  }

  const date = new Date(dateString);
  return !isNaN(date);
}
