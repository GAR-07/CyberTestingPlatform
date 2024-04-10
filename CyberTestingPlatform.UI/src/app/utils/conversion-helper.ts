export function convertImgPathToTag(apiUrl: string, data: string): string {
  const imageRegex = /Resources\\Images\\[^\.]+[^\s]+/g;
  
  data = data.replace(imageRegex, (path) => {
    return `<div class="content-img-wrapper"><img src="${apiUrl}/${path}" alt="Изображение"></div>`;
  });
  return data.replace(/\n/g, '<br>');
}

export function convertTagToImgPath(apiUrl: string, data: string): string {
  const imageRegex = new RegExp(`<div\\s+class="content-img-wrapper"><img\\s+src="${apiUrl}/([^"]+)"\\s+alt="Изображение"><\/div>`, "g");
  
  data = data.replace(imageRegex, (match, src) => {
    return src;
  });
  return data.replace(/<br>/g, '\n');
}