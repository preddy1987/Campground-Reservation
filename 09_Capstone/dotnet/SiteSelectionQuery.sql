select * from site 
LEFT OUTER JOIN (
SELECT DISTINCT q.site_number as site_conflict
 FROM (SELECT site_number,
	site.site_id,
	site.campground_id,
	site.max_occupancy,
	site.accessible,
	site.max_rv_length,
	site.utilities,
	reservation.from_date,
	reservation.to_date
FROM site JOIN reservation ON site.site_id = reservation.site_id
WHERE (campground_id = 3
		AND
			 ('5/1/2019' BETWEEN reservation.from_date AND reservation.to_date
				OR
			'5/8/2019' BETWEEN reservation.from_date AND reservation.to_date))) as q) as r
		ON site.site_number = r.site_conflict
		WHERE campground_id = 3;