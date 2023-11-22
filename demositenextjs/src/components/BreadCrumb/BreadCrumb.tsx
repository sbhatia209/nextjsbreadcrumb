import Link from 'next/link';
import { useSitecoreContext } from '@sitecore-jss/sitecore-jss-nextjs';

interface Breadcrumb {
  pageTitle: string;
  url: string;
}

const BreadCrumb = (): JSX.Element => {
  const { sitecoreContext } = useSitecoreContext();
  const BreadcrumbData = sitecoreContext.Breadcrumbs as Breadcrumb[];
  const urlArray = BreadcrumbData?.map((crumb: Breadcrumb) => crumb.url);

  if (BreadcrumbData?.length) {
    return (
      <nav className={styles.breadcrumb}>
        {
          <ul>
            {BreadcrumbData?.map(
              (crumb: Breadcrumb, index: number) =>
                crumb?.pageTitle && (
                  <li key={index}>
                    {index === BreadcrumbData?.length - 1 ? (
                      <span>{crumb?.pageTitle}</span>
                    ) : (
                      <Link
                        href={`${urlArray.slice(index, index + 1).join('/')}`} >
                        {crumb?.pageTitle}
                      </Link>
                    )}
                  </li>
                )
            )}
          </ul>
        }
      </nav>
    );
  }

  return <></>;
};

export default BreadCrumb;