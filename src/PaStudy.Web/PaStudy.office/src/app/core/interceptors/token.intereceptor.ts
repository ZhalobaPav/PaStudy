import { HttpInterceptorFn } from '@angular/common/http';
import { TOKEN_KEY } from '../../shared/contsants/base.constants';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem(TOKEN_KEY);
  const authReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      })
    : req;

  return next(authReq);
};
