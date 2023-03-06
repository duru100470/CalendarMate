const baseLink: string = `https://${document.location.hostname}:5000`;

function fetchGet(url: string) {
  return fetch(baseLink + url, {
    method: "GET",
    mode: "cors",
    credentials: "include"
  });
}

function fetchPost(url: string, data: any) {
  console.log(`${url}\n${JSON.stringify(data)}`);
  return fetch(baseLink + url, {
    method: "POST",
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
    credentials: "include"
  });
}

function fetchPut(url: string, data: any) {
  console.log(`${url}\n${JSON.stringify(data)}`);
  return fetch(baseLink + url, {
    method: "PUT",
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
    credentials: "include"
  });
}

function fetchDelete(url: string) {
  return fetch(baseLink + url, {
    method: "DELETE",
    mode: "cors",
    credentials: "include"
  });
}

export {
    fetchGet,
    fetchPost,
    fetchPut,
    fetchDelete
}